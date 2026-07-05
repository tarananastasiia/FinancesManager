using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
