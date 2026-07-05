namespace Application.Interfaces
{
    public interface IStripeCustomerService
    {
        Task<string> CreateCustomerAsync(string email, string name);
    }
}
