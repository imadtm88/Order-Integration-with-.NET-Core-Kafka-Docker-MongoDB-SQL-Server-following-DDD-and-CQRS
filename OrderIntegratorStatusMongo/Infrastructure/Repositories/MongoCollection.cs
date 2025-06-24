using Domain.Entities;
using MongoDB.Driver;
namespace Infrastructure.Repositories
{
    public class MongoCollection
    {
        private readonly IMongoCollection<Order> _orderCollection;
        public MongoCollection(IMongoClient mongoClient, string databaseName, string collectionName)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _orderCollection = database.GetCollection<Order>(collectionName);
        }
        public async Task AddOrderAsync(Order order)
        {
            await _orderCollection.InsertOneAsync(order);
        }
    }
}