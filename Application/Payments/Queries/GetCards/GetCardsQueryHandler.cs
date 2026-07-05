using Application.DTOs.ResponseModel.Payment;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Application.Payments.Queries.GetCards
{
    public class GetCardsQueryHandler : IRequestHandler<GetCardsQuery, List<CardResponse>>
    {
        private readonly IApplicationDbContext _db;

        public GetCardsQueryHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<CardResponse>> Handle(
            GetCardsQuery request,
            CancellationToken cancellationToken)
        {
            var service = new ChargeService();
            var user = await _db.Users.FirstAsync(x => x.Id == request.UserId);
            var customerId = user.StripeCustomerId;

            var charges = await service.ListAsync(
                new ChargeListOptions
                {
                    Limit = 10,
                    Customer = customerId
                },
                cancellationToken: cancellationToken);

            return charges.Data
                .Where(c => c.PaymentMethodDetails?.Card != null)
                .Select(c => new CardResponse
                {
                    Brand = c.PaymentMethodDetails!.Card!.Brand,
                    Last4 = c.PaymentMethodDetails.Card.Last4,
                    ExpMonth = c.PaymentMethodDetails.Card.ExpMonth,
                    ExpYear = c.PaymentMethodDetails.Card.ExpYear
                }).ToList();
        }
    }
}
