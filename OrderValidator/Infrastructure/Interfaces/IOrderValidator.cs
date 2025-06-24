using Infrastructure.Model;

namespace Infrastructure.Interfaces
{
    public interface IOrderValidator
    {
        bool Validate(Order order, out string reason);
        void SetReferencePrice(double referencePrice);
        bool ValidateShippingAddress(Order order, out string reason);
        bool ValidateEmail(Order order, out string reason);
    }
}