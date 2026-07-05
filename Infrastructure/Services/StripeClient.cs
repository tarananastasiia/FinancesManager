using Stripe;

namespace Infrastructure.Services
{
    public interface IStripeClient
    {
        Task<List<PaymentMethod>> GetCardMethods(string customerId);
        Task<List<PaymentIntent>> GetPaymentHistory(string customerId, int limit);
    }
    public class StripeClient : IStripeClient
    {
        public async Task<List<PaymentMethod>> GetCardMethods(string customerId)
        {
            var service = new PaymentMethodService();

            var result = await service.ListAsync(new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = "card"
            });

            return result.Data;
        }

        public async Task<List<PaymentIntent>> GetPaymentHistory(string customerId, int limit)
        {
            var service = new PaymentIntentService();

            var result = await service.ListAsync(new PaymentIntentListOptions
            {
                Customer = customerId,
                Limit = limit
            });

            return result.Data;
        }
    }
}
