using Infrastructure.Interfaces;
using Infrastructure.Model;

namespace Application.Communication
{
    public class OrderPaymentValidator : IOrderPaymentValidator
    {
        public bool IsValid(OrderModel order, out string validationMessage)
        {
            validationMessage = string.Empty;

            if (order == null)
            {
                validationMessage = "Order is null.";
                return false;
            }

            DateTime currentDate = DateTime.Now;
            DateTime creationDate = order.CreationDate;

            if (creationDate > currentDate)
            {
                validationMessage = "Order creation date is in the future.";
                return false;
            }

            if ((currentDate - creationDate).TotalDays > 5)
            {
                validationMessage = "Order creation date is older than 5 days.";
                return false;
            }

            return true;
        }
    }
}