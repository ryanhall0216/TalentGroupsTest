namespace TalentGroupsTest.Models
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
    public class MongoDbSettings: IMongoDbSettings
    {
        public string DatabaseName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }
}
