using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<ApplicationUser> _hasher = new();

        public string Hash(ApplicationUser user, string password)
            => _hasher.HashPassword(user, password);

        public bool Verify(ApplicationUser user, string password)
            => _hasher.VerifyHashedPassword(user, user.PasswordHash, password)
               == PasswordVerificationResult.Success;
    }
}
