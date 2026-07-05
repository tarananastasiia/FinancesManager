using Application.DTOs.ResponseModel.Payment;

namespace Infrastructure.Services
{
    public interface IPaymentService
    {
        Task<List<CardResponse>> GetCards(string customerId);
        Task<List<PaymentHistoryResponse>> GetHistory(string customerId);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IStripeClient _stripe;

        public PaymentService(IStripeClient stripe)
        {
            _stripe = stripe;
        }

        public async Task<List<CardResponse>> GetCards(string customerId)
        {
            var methods = await _stripe.GetCardMethods(customerId);

            return methods.Select(pm => new CardResponse
            {
                Brand = pm.Card.Brand,
                Last4 = pm.Card.Last4,
                ExpMonth = pm.Card.ExpMonth,
                ExpYear = pm.Card.ExpYear
            }).ToList();
        }

        public async Task<List<PaymentHistoryResponse>> GetHistory(string customerId)
        {
            var payments = await _stripe.GetPaymentHistory(customerId, 20);

            return payments.Select(p => new PaymentHistoryResponse
            {
                Id = p.Id,
                Amount = p.Amount / 100.0,
                Currency = p.Currency,
                Status = p.Status,
                Date = DateTimeOffset.FromUnixTimeSeconds(p.Created.Second).UtcDateTime
            }).ToList();
        }
    }
}
