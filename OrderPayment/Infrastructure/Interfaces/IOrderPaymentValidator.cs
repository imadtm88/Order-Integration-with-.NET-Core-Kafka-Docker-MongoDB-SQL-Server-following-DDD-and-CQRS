using Infrastructure.Model;

namespace Infrastructure.Interfaces
{
    public interface IOrderPaymentValidator
    {
        bool IsValid(OrderModel order, out string validationMessage);
    }
}
