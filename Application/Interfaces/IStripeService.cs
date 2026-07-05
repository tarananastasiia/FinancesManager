namespace Application.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreatePaymentIntentAsync(long amount, string currency);
    }
}
