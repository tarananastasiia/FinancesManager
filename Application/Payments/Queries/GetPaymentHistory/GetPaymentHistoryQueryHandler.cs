using Application.DTOs.ResponseModel.Payment;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Application.Payments.Queries.GetPaymentHistory
{
    public class GetPaymentHistoryQueryHandler : IRequestHandler<GetPaymentHistoryQuery, List<PaymentHistoryResponse>>
    {
        private readonly IApplicationDbContext _db;

        public GetPaymentHistoryQueryHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<PaymentHistoryResponse>> Handle(
            GetPaymentHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var service = new ChargeService();

            var user = await _db.Users.FirstAsync(x => x.Id == request.UserId);
            var customerId = user.StripeCustomerId;

            var charges = await service.ListAsync(
                new ChargeListOptions
                {
                    Limit = 20,
                    Customer = customerId
                },
                cancellationToken: cancellationToken);

            return charges.Data.Select(c => new PaymentHistoryResponse
            {
                Id = c.Id,
                Amount = c.Amount / 100.0,
                Currency = c.Currency,
                Status = c.Status,
                Date = DateTimeOffset
                    .FromUnixTimeSeconds(c.Created.Second)
                    .UtcDateTime,
                Card = c.PaymentMethodDetails?.Card?.Last4
            }).ToList();
        }
    }
}
