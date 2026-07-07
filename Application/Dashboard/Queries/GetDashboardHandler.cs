using Application.Common;
using Application.DTOs;
using Application.DTOs.ResponseModel.Dashboard;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Application.Dashboard.Queries
{
    public class GetDashboardHandler : IRequestHandler<GetDashboardQuery, DashboardResponse>
    {
        private readonly IApplicationDbContext _db;
        private readonly PaymentCategoryResolver _categoryResolver;

        public GetDashboardHandler(IApplicationDbContext db, PaymentCategoryResolver categoryResolver)
        {
            _db = db;
            _categoryResolver = categoryResolver;
        }

        public async Task<DashboardResponse> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .FirstAsync(x => x.Id == request.UserId, cancellationToken);

            var stripe = new ChargeService();

            var charges = await stripe.ListAsync(
                new ChargeListOptions
                {
                    Customer = user.StripeCustomerId,
                    Limit = 100
                },
                cancellationToken: cancellationToken);

            var spentMonth = charges.Data
                .Where(x =>
                    x.Created.Month == DateTime.UtcNow.Month &&
                    x.Created.Year == DateTime.UtcNow.Year)
                .Sum(x => x.Amount) / 100m;

            var monthlySpending = charges.Data
                .GroupBy(x => new
                {
                    x.Created.Year,
                    x.Created.Month
                })
                .Select(x => new MonthlySpendingDto
                {
                    Month = $"{x.Key.Month}/{x.Key.Year}",
                    Amount = x.Sum(c => c.Amount) / 100m
                })
                .OrderBy(x => x.Month)
                .ToList();

            var categories = charges.Data
                .GroupBy(x => _categoryResolver.GetCategory(x.Description))
                .Select(x => new CategorySpendingDto
                {
                    Category = x.Key.ToString(),
                    Amount = x.Sum(c => c.Amount) / 100m
                })
                .ToList();

            return new DashboardResponse
            {
                Balance = 10000 - spentMonth,
                SpentThisMonth = spentMonth,
                Transactions = charges.Data.Count,
                Cards = charges.Data
                    .Select(x => x.PaymentMethod)
                    .Distinct()
                    .Count(),
                MonthlySpending = monthlySpending,
                CategorySpending = categories
            };
        }
    }
}
