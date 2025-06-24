using Domain.Model;
using Domain.Model.Enum;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(OrderModel order);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel> GetOrderAsync(string Id);
        Task UpdateOrderStatus(string orderId, OrderStatus newStatus);
        Task UpdateOrderAsync(OrderModel order);
    }
}