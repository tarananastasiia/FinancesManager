using Application.DTOs.RequestModel;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FinancesManager.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapGet("/me", Me).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> Register(
        RegisterModel model,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext db,
        JwtService jwtService)
    {
        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            if (await userManager.FindByEmailAsync(model.Email) != null)
            {
                return Results.BadRequest(new { message = "Email already registered" });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Results.BadRequest(new { message = "Registration failed", errors });
            }

            var token = jwtService.GenerateToken(user);

            await transaction.CommitAsync();

            return Results.Ok(new
            {
                token,
                user.Email,
                user.FullName
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Results.Problem("Registration failed: " + ex.Message);
        }
    }

    private static async Task<IResult> Login(
        LoginModel model,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtService jwtService)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return Results.Unauthorized();

        var result = await signInManager.CheckPasswordSignInAsync(
            user,
            model.Password,
            true);

        if (!result.Succeeded)
            return Results.Unauthorized();

        var token = jwtService.GenerateToken(user);

        return Results.Ok(new
        {
            token,
            user.Email,
            user.FullName
        });
    }

    private static async Task<IResult> Me(
        UserManager<ApplicationUser> userManager,
        ClaimsPrincipal user)    
    {
        var appUser = await userManager.GetUserAsync(user);

        if (appUser is null)
            return Results.Unauthorized();

        return Results.Ok(new
        {
            appUser.Id,
            appUser.Email,
            appUser.FullName
        });
    }
}