using Application.Interfaces;
using Stripe;

namespace Infrastructure.Services
{
    public class StripeService : IStripeService
    {
        private readonly ChargeService _chargeService;
        public StripeService(ChargeService chargeService)
        {
            _chargeService = chargeService;
        }

        public async Task<string> CreateCustomerAsync(string email, string fullName)
        {
            var service = new CustomerService();

            var customer = await service.CreateAsync(new CustomerCreateOptions
            {
                Email = email,
                Name = fullName
            });

            return customer.Id;
        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(PaymentIntentCreateOptions options)
        {
            var service = new PaymentIntentService();

            var result = await service.CreateAsync(options);

            return result;
        }

        public async Task<List<Charge>> GetChargesAsync(string customerId, int limit)
        {
            var charges = await _chargeService.ListAsync(
                new ChargeListOptions
                {
                    Customer = customerId,
                    Limit = limit
                });

            return charges.Data;
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsAsync(string customerId)
        {
            var service = new PaymentMethodService();

            var methods = await service.ListAsync(
                new PaymentMethodListOptions
                {
                    Customer = customerId,
                    Type = "card"
                });

            return methods.Data;
        }
    }
}
