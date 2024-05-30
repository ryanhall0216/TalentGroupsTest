using Amazon.Runtime.Internal.Util;
using TalentGroupsTest.Data.Repository.Interface;
using TalentGroupsTest.Models;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TalentGroupsTest.Data.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    { 
        private readonly IMongoClient _client;
        private readonly IMongoCollection<TDocument> _collection;
        private readonly IMemoryCache _cache;
        private readonly string _cacheName;


        public MongoRepository(IMongoDbSettings settings, IMemoryCache cache)
        {
            _client = new MongoClient(settings.ConnectionString);

            var database = _client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
            _cache = cache;
            _cacheName = _collection.CollectionNamespace.CollectionName;
        }

        private string GetCollectionName(Type documentType)
        {
            var attribute = documentType.GetCustomAttribute<BsonCollectionAttribute>();
            return attribute?.CollectionName ?? string.Empty;
        }

        public virtual IMongoQueryable<TDocument> GetQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual async Task<IEnumerable<TDocument>> FindAllAsync(IMongoQueryable<TDocument> collectionQuery)
        {
            var cacheEntry = await _cache.GetOrCreateAsync($"{_cacheName}", async entry =>
            {
                entry.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(15));
                return await collectionQuery.ToListAsync();
            });

            return cacheEntry;
        }

        public virtual async Task<bool> InsertOneAsync(TDocument document)
        {
            try
            {
                await _collection.InsertOneAsync(document);
                _cache.Remove($"{_cacheName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the document: {ex.Message}");
                return false;
            }
        }

        public virtual async Task<bool> ReplaceOneAsync(Expression<Func<TDocument, bool>> filterExpression, TDocument document)
        {
            var result = await _collection.ReplaceOneAsync(filterExpression, document);
            _cache.Remove($"{_cacheName}");
            return true;
        }

        public async Task<bool> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = await _collection.DeleteOneAsync(filterExpression);
            _cache.Remove($"{_cacheName}");
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
