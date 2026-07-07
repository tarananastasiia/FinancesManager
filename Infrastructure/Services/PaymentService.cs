using Application.DTOs.ResponseModel.Payment;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IStripeService _stripe;

        public PaymentService(IStripeService stripe)
        {
            _stripe = stripe;
        }

        public async Task<List<CardResponse>> GetCardsAsync(string customerId)
        {
            var methods = await _stripe.GetPaymentMethodsAsync(customerId);

            var result = methods.Select(pm => new CardResponse
            {
                Brand = pm.Card.Brand,
                Last4 = pm.Card.Last4,
                ExpMonth = pm.Card.ExpMonth,
                ExpYear = pm.Card.ExpYear
            }).ToList();

            return result;
        }

        public async Task<List<PaymentHistoryResponse>> GetHistoryAsync(string customerId)
        {
            var payments = await _stripe.GetChargesAsync(customerId, 20);

            var result = payments.Select(p => new PaymentHistoryResponse
            {
                Id = p.Id,
                Amount = p.Amount / 100.0,
                Currency = p.Currency,
                Status = p.Status,
                Date = p.Created.ToUniversalTime(),
                Description = p.Description
            }).ToList();

            return result;
        }
    }
}
