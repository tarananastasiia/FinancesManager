using Stripe;

namespace Application.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreateCustomerAsync(string email, string name);
        Task<PaymentIntent> CreatePaymentIntentAsync(PaymentIntentCreateOptions options);
        Task<List<Charge>> GetChargesAsync(string customerId, int limit);
        Task<List<PaymentMethod>> GetPaymentMethodsAsync(string customerId);
    }
}
