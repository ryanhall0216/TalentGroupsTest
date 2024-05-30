using TalentGroupsTest.Models;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace TalentGroupsTest.Data.Repository.Interface
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IMongoQueryable<TDocument> GetQueryable();
        Task<IEnumerable<TDocument>> FindAllAsync(IMongoQueryable<TDocument> query);
        Task<bool> InsertOneAsync(TDocument document);
        Task<bool> ReplaceOneAsync(Expression<Func<TDocument, bool>> filterExpression, TDocument document);
        Task<bool> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
