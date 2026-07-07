using Application.Common.Constants;
using Application.DTOs.ResponseModel.Auth;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
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

        var days = model.RememberMe
            ? AuthConstants.RememberMeRefreshTokenDays
            : AuthConstants.DefaultRefreshTokenDays;

        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == model.Email, cancellationToken);

        if (user == null)
            throw new UnauthorizedException("Invalid credentials");

        var isValid = _passwordService.Verify(user, model.Password);

        if (!isValid)
            throw new UnauthorizedException("Invalid credentials");

        var accessToken = _jwtService.GenerateToken(user);

        var refreshToken = new RefreshToken
        {
            Token = _jwtService.GenerateRefreshToken(),
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(days)
        };

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync(cancellationToken);

        return new LoginResponse
        {
            Success = true,
            Token = accessToken,
            RefreshToken = refreshToken.Token,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}