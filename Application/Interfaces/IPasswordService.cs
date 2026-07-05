using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPasswordService
    {
        string Hash(ApplicationUser user, string password);
        bool Verify(ApplicationUser user, string password);
    }
}
