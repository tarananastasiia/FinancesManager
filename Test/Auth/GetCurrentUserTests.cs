using Application.Auth.Queries;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Auth
{
    [TestClass]
    public class GetCurrentUserTests
    {
        [TestMethod]
        public async Task Should_Return_User()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ApplicationDbContext(options);

            var userId = Guid.NewGuid();

            db.Users.Add(new ApplicationUser
            {
                Id = userId,
                Email = "test@mail.com",
                FullName = "User"
            });

            await db.SaveChangesAsync();

            var handler = new GetCurrentUserQueryHandler(db);

            var result = await handler.Handle(
                new GetCurrentUserQuery(userId.ToString()),
                CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual("test@mail.com", result.Email);
            Assert.AreEqual("User", result.FullName);
            Assert.AreEqual(userId.ToString(), result.UserId);
        }
    }
}