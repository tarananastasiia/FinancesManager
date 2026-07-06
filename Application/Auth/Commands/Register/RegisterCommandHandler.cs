using Application.DTOs.ResponseModel.Auth;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Application.Auth.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IApplicationDbContext _db;
    private readonly IJwtService _jwtService;
    private readonly IPasswordService _passwordService;

    public RegisterCommandHandler(
        IApplicationDbContext db,
        IJwtService jwtService,
        IPasswordService passwordService)
    {
        _db = db;
        _jwtService = jwtService;
        _passwordService = passwordService;
    }

    public async Task<RegisterResponse> Handle(
    RegisterCommand request,
    CancellationToken cancellationToken)
    {
        var model = request.Model;

        var exists = await _db.Users
            .AnyAsync(x => x.Email == model.Email, cancellationToken);

        if (exists)
        {
            return new RegisterResponse
            {
                Success = false,
                Error = "Email already registered"
            };
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            FullName = model.FullName
        };

        user.PasswordHash = _passwordService.Hash(user, model.Password);

        var customerService = new CustomerService();

        var stripeCustomer = await customerService.CreateAsync(new CustomerCreateOptions
        {
            Email = user.Email,
            Name = user.FullName
        }, cancellationToken: cancellationToken);

        user.StripeCustomerId = stripeCustomer.Id;

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        var token = _jwtService.GenerateToken(user);

        return new RegisterResponse
        {
            Success = true,
            Token = token,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}