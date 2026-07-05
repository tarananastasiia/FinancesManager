using Application.Payments.Commands.CreatePayment;
using Application.Payments.Queries.GetCards;
using Application.Payments.Queries.GetPaymentHistory;
using MediatR;
using System.Security.Claims;

namespace FinancesManager.Endpoints;

public static class PaymentEndpoints
{
    public static RouteGroupBuilder MapPaymentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/payments")
            .RequireAuthorization();

        group.MapPost("/create-intent", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new CreatePaymentCommand())));

        group.MapGet("/history", async (IMediator mediator, HttpContext ctx) =>
        {
            var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Results.Unauthorized();

            var query = new GetPaymentHistoryQuery
            {
                UserId = Guid.Parse(userId)
            };

            return Results.Ok(await mediator.Send(query));
        });

        group.MapGet("/cards", async (IMediator mediator, HttpContext ctx) =>
        {
            var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Results.Unauthorized();

            var query = new GetCardsQuery
            {
                UserId = Guid.Parse(userId)
            };

            return Results.Ok(await mediator.Send(query));
        });

        return group;
    }
}