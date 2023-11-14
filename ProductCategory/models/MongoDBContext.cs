using MongoDB.Driver;

namespace ProductCategoryAPI.models
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public MongoDBContext(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }
        public MongoClient GetClient() {  return _client; }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
    }
}