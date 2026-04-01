using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
