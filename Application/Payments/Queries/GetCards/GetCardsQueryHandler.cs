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
        private readonly IStripeService _stripeService;

        public GetCardsQueryHandler(IApplicationDbContext db, IStripeService stripeService)
        {
            _db = db;
            _stripeService = stripeService;
        }

        public async Task<List<CardResponse>> Handle(
            GetCardsQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _db.Users.FirstAsync(x => x.Id == request.UserId);
            var customerId = user.StripeCustomerId;

            var paymentMethods = await _stripeService.GetPaymentMethodsAsync(user.StripeCustomerId);

            return paymentMethods.Select(pm => new CardResponse
            {
                Brand = pm.Card.Brand,
                Last4 = pm.Card.Last4,
                ExpMonth = pm.Card.ExpMonth,
                ExpYear = pm.Card.ExpYear
            }).ToList();
        }
    }
}
