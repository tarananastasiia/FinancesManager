using Application.DTOs.RequestModel;
using Application.DTOs.ResponseModel.Auth;
using MediatR;

namespace Application.Auth.Commands.Login
{
    public record LoginCommand(LoginModel Model) : IRequest<LoginResponse>;
}
