using Application.DTOs.ResponseModel.Auth;
using MediatR;
using System.Security.Claims;

namespace Application.Auth.Queries
{
    public record GetCurrentUserQuery(string UserId)
    : IRequest<GetCurrentUserResponse>;
}
