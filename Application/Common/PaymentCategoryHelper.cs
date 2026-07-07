using Domain.Enums;

namespace Application.Common
{
    public class PaymentCategoryHelper
    {
        public static PaymentCategory GetCategory(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return PaymentCategory.Other;

            var value = description.ToLowerInvariant();

            if (value.Contains("uber") || value.Contains("bolt"))
                return PaymentCategory.Transport;

            if (value.Contains("spotify") || value.Contains("netflix"))
                return PaymentCategory.Entertainment;

            if (value.Contains("amazon"))
                return PaymentCategory.Shopping;

            if (value.Contains("shell"))
                return PaymentCategory.Fuel;

            if (value.Contains("starbucks") ||
                value.Contains("mcdonald") ||
                value.Contains("kfc"))
                return PaymentCategory.Food;

            return PaymentCategory.Other;
        }
    }
}
