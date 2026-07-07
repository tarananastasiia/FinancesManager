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

        public async Task<PaymentIntent> CreatePaymentIntentAsync(PaymentIntentCreateOptions options, CancellationToken cancellationToken)
        {
            var service = new PaymentIntentService();

            return await service.CreateAsync(
                options,
                cancellationToken: cancellationToken);
        }

        public async Task<List<Charge>> GetChargesAsync(string customerId, int limit, CancellationToken cancellationToken)
        {
            var charges = await _chargeService.ListAsync(
                new ChargeListOptions
                {
                    Customer = customerId,
                    Limit = limit
                },
                cancellationToken: cancellationToken);

            return charges.Data;
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsAsync(string customerId, CancellationToken cancellationToken)
        {
            var service = new PaymentMethodService();

            var methods = await service.ListAsync(
                new PaymentMethodListOptions
                {
                    Customer = customerId,
                    Type = "card"
                },
                cancellationToken: cancellationToken);

            return methods.Data;
        }
    }
}
