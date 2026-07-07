using Stripe;

namespace Application.Interfaces
{
    public interface IStripeService
    {
        Task<List<PaymentMethod>> GetCardMethodsAsync(string customerId);
        Task<List<PaymentIntent>> GetPaymentHistoryAsync(string customerId, int limit);
        Task<string> CreateCustomerAsync(string email, string name);
        Task<PaymentIntent> CreatePaymentIntentAsync(PaymentIntentCreateOptions options, CancellationToken cancellationToken);
        Task<List<Charge>> GetChargesAsync(string customerId, int limit, CancellationToken cancellationToken);
        Task<List<PaymentMethod>> GetPaymentMethodsAsync(string customerId, CancellationToken cancellationToken);
    }
}
