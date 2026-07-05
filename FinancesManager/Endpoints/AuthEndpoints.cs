using Application.Auth.Commands.Login;
using Application.Auth.Commands.Register;
using Application.Auth.Queries;
using Application.DTOs.RequestModel;
using MediatR;
using System.Security.Claims;

namespace FinancesManager.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", async (RegisterModel model, IMediator mediator) =>
            await mediator.Send(new RegisterCommand(model)));

        group.MapPost("/login", async (LoginModel model, IMediator mediator) =>
            await mediator.Send(new LoginCommand(model)));

        group.MapGet("/me", async (
            ClaimsPrincipal user,
            IMediator mediator) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            return await mediator.Send(new GetCurrentUserQuery(userId!));
        })
        .RequireAuthorization();

        return group;
    }
}