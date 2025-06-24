using Infrastructure.Model;

namespace Infrastructure.Interfaces
{
    public interface IOrderPaymentHttp
    {
        Task<OrderModel?> GetOrderAsync(string Id);
    }
}
