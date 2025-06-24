using Infrastructure.Model;

namespace Infrastructure.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrdersAsync(string Id);
    }
}