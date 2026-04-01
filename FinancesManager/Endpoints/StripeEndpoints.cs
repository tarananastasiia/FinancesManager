using Microsoft.AspNetCore.Authorization;
using Stripe;

namespace FinancesManager.Endpoints;

public static class StripeEndpoints
{
    public static void MapStripeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/payments")
                       .RequireAuthorization();

        group.MapPost("/create-intent", CreatePaymentIntent);
        group.MapGet("/history", GetHistory);
        group.MapGet("/cards", GetCards);
    }

    private static async Task<IResult> CreatePaymentIntent()
    {
        var service = new PaymentIntentService();

        var intent = await service.CreateAsync(
            new PaymentIntentCreateOptions
            {
                Amount = 2000,
                Currency = "usd",
                AutomaticPaymentMethods = new()
                {
                    Enabled = true
                }
            });

        return Results.Ok(new
        {
            clientSecret = intent.ClientSecret
        });
    }

    private static async Task<IResult> GetHistory()
    {
        var service = new ChargeService();

        var charges = await service.ListAsync(
            new ChargeListOptions
            {
                Limit = 20
            });

        var result = charges.Data.Select(c => new
        {
            c.Id,
            Amount = c.Amount / 100.0,
            c.Currency,
            c.Status,
            Date = DateTimeOffset
                    .FromUnixTimeSeconds(c.Created.Second)
                    .DateTime,
            Card = c.PaymentMethodDetails
                    ?.Card?.Last4
        });

        return Results.Ok(result);
    }

    private static async Task<IResult> GetCards()
    {
        var service = new ChargeService();

        var charges = await service.ListAsync(new ChargeListOptions
        {
            Limit = 10
        });

        var cards = charges.Data
            .Where(c => c.PaymentMethodDetails?.Card != null)
            .Select(c => new
            {
                Brand = c.PaymentMethodDetails.Card.Brand,
                Last4 = c.PaymentMethodDetails.Card.Last4,
                ExpMonth = c.PaymentMethodDetails.Card.ExpMonth,
                ExpYear = c.PaymentMethodDetails.Card.ExpYear
            });

        return Results.Ok(cards);
    }
}