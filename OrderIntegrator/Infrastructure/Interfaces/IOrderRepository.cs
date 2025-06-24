using Domain.Entity;

namespace Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        Task SaveOrderAsync(Order order);
    }
}
