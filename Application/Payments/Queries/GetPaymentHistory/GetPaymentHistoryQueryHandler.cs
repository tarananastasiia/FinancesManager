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

        public GetPaymentHistoryQueryHandler(IApplicationDbContext db, IStripeService stripeService)
        {
            _db = db;
            _stripeService = stripeService;
        }

        public async Task<List<PaymentHistoryResponse>> Handle(
            GetPaymentHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _db.Users.FirstAsync(x => x.Id == request.UserId);
            var customerId = user.StripeCustomerId;

            var charges = await _stripeService.GetChargesAsync(user.StripeCustomerId, 20, cancellationToken);

            return charges.Select(c => new PaymentHistoryResponse
            {
                Id = c.Id,
                Amount = c.Amount / 100.0,
                Currency = c.Currency,
                Status = c.Status,
                Date = c.Created.ToUniversalTime(),
                Card = c.PaymentMethodDetails?.Card?.Last4,
                Description = c.Description,
                Category = PaymentCategoryHelper.GetCategory(c.Description).ToString()
            }).ToList();
        }
    }
}
