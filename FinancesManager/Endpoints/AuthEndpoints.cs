using Application.Auth.Commands.Login;
using Application.Auth.Commands.Register;
using Application.Auth.Queries;
using Application.DTOs.RequestModel;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        group.MapPost("/refresh", async (
        RefreshRequest request,
        IApplicationDbContext db,
        IJwtService jwt) =>
        {
            var token = await db.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (token == null || !token.IsActive)
                return Results.Unauthorized();

            var newAccessToken = jwt.GenerateToken(token.User);

            return Results.Ok(new
            {
                accessToken = newAccessToken
            });
        });

        group.MapPost("/logout", async (
        RefreshRequest request,
        IApplicationDbContext db,
        CancellationToken cancellationToken) =>
        {
            var token = await db.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            if (token != null)
            {
                token.Revoked = DateTime.UtcNow;
                await db.SaveChangesAsync(cancellationToken);
            }

            return Results.Ok();
        });

        return group;
    }
}