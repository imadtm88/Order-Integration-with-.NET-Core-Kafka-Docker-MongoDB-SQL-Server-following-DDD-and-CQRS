using Domain.Entity;

namespace Infrastructure.Interfaces
{
    public interface IOrderIntegrationHttp
    {
        Task<Order?> GetOrderAsync(string Id);
    }
}
