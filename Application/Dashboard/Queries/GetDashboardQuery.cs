using Application.DTOs.ResponseModel.Dashboard;
using MediatR;

namespace Application.Dashboard.Queries
{
    public class GetDashboardQuery : IRequest<DashboardResponse>
    {
        public Guid UserId { get; set; }
    }
}
