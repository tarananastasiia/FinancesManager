using Application.Common.Settings;
using Domain.Enums;
using Microsoft.Extensions.Options;

namespace Application.Common
{
    public class PaymentCategoryResolver
    {
        private readonly PaymentCategoryResolverSettings _settings;

        public PaymentCategoryResolver(
            IOptions<PaymentCategoryResolverSettings> options)
        {
            _settings = options.Value;
        }

        public PaymentCategoryType GetCategory(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return PaymentCategoryType.Other;

            var value = description.ToLowerInvariant();

            foreach (var rule in _settings.Rules)
            {
                if (rule.Keywords.Any(x => value.Contains(x)))
                {
                    return Enum.Parse<PaymentCategoryType>(
                        rule.Category);
                }
            }

            return PaymentCategoryType.Other;
        }
    }
}
