using Stripe;

namespace Application.Interfaces
{
    public interface IStripeChargeService
    {
        Task<IReadOnlyList<Charge>> GetChargesAsync(string customerId, int limit);
    }
}
