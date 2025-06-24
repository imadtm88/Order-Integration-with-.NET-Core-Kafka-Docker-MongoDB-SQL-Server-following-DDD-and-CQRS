using Domain.Model;
using Domain.Model.Enum;
using Domain.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class MongoDBOrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<OrderModel> _orders;

        public MongoDBOrderRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("OrderDb");
            _orders = database.GetCollection<OrderModel>("Orders");
        }

        public async Task AddOrderAsync(OrderModel order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            return await _orders.Find(_ => true).ToListAsync();
        }

        public async Task<OrderModel> GetOrderAsync(string Id)
        {
            return await _orders.Find(order => order.Id == Id).FirstOrDefaultAsync();
        }

        public async Task UpdateOrderStatus(string orderId, OrderStatus newStatus)
        {
            var filter = Builders<OrderModel>.Filter.Eq(order => order.Id, orderId);
            var update = Builders<OrderModel>.Update.Set(order => order.OrderStatus, newStatus);
            await _orders.UpdateOneAsync(filter, update);
        }

        public async Task UpdateOrderAsync(OrderModel order)
        {
            var filter = Builders<OrderModel>.Filter.Eq(o => o.Id, order.Id);
            await _orders.ReplaceOneAsync(filter, order);
        }
    }
}