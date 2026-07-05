using Application.DTOs.ResponseModel.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IJwtService _jwtService;
    private readonly IApplicationDbContext _db;
    private readonly IPasswordService _passwordService;

    public LoginCommandHandler(
        IJwtService jwtService,
        IApplicationDbContext db,
        IPasswordService passwordService)
    {
        _jwtService = jwtService;
        _db = db;
        _passwordService = passwordService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;

        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == model.Email, cancellationToken);

        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Error = "Invalid credentials"
            };
        }

        var isValid = _passwordService.Verify(user, model.Password);

        if (!isValid)
        {
            return new LoginResponse
            {
                Success = false,
                Error = "Invalid credentials"
            };
        }

        var token = _jwtService.GenerateToken(user);

        return new LoginResponse
        {
            Success = true,
            Token = token,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}