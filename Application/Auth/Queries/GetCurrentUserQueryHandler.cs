using Application.DTOs.ResponseModel.Auth;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Queries;

public class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    private readonly IApplicationDbContext _db;

    public GetCurrentUserQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<GetCurrentUserResponse> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId, cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found");

        return new GetCurrentUserResponse
        {
            UserId = user.Id.ToString(),
            Email = user.Email,
            FullName = user.FullName
        };
    }
}