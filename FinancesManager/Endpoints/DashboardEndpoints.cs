using Application.Dashboard.Queries;
using MediatR;
using System.Security.Claims;

namespace FinancesManager.Endpoints
{
    public static class DashboardEndpoints
    {
        public static RouteGroupBuilder MapDashboardEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/dashboard")
                .RequireAuthorization();

            group.MapGet("/", async (ClaimsPrincipal user, IMediator mediator) =>
            {
                var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

                return await mediator.Send(new GetDashboardQuery
                {
                    UserId = Guid.Parse(id!)
                });
            });

            return group;
        }
    }
}
