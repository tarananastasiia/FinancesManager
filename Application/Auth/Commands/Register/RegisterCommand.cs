using Application.DTOs.RequestModel;
using Application.DTOs.ResponseModel.Auth;
using MediatR;

namespace Application.Auth.Commands.Register
{
    public record RegisterCommand(RegisterModel Model)
        : IRequest<RegisterResponse>;
}
