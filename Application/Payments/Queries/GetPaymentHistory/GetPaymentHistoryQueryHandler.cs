using Application.Common;
using Application.DTOs.ResponseModel.Payment;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Payments.Queries.GetPaymentHistory
{
    public class GetPaymentHistoryQueryHandler : IRequestHandler<GetPaymentHistoryQuery, List<PaymentHistoryResponse>>
    {
        private readonly IApplicationDbContext _db;
        private readonly IStripeService _stripeService;
        private readonly PaymentCategoryResolver _categoryResolver;

        public GetPaymentHistoryQueryHandler(IApplicationDbContext db, IStripeService stripeService, PaymentCategoryResolver categoryResolver)
        {
            _db = db;
            _stripeService = stripeService;
            _categoryResolver = categoryResolver;
        }

        public async Task<List<PaymentHistoryResponse>> Handle(
            GetPaymentHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _db.Users.FirstAsync(x => x.Id == request.UserId);
            var customerId = user.StripeCustomerId;

            var charges = await _stripeService.GetChargesAsync(user.StripeCustomerId, 20);

            var result = charges.Select(c => new PaymentHistoryResponse
            {
                Id = c.Id,
                Amount = c.Amount / 100.0,
                Currency = c.Currency,
                Status = c.Status,
                Date = c.Created.ToUniversalTime(),
                Card = c.PaymentMethodDetails?.Card?.Last4,
                Description = c.Description,
                Category = _categoryResolver.GetCategory(c.Description).ToString()
            }).ToList();

            return result;
        }
    }
}
