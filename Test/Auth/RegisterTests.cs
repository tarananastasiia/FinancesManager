using Application.Auth.Commands.Register;
using Application.DTOs.RequestModel;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Auth
{
    [TestClass]
    public class RegisterTests
    {
        [TestMethod]
        public async Task Register_Should_Fail_When_Email_Exists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ApplicationDbContext(options);

            db.Users.Add(new ApplicationUser
            {
                Email = "test@mail.com",
                FullName = "Existing"
            });

            await db.SaveChangesAsync();

            var handler = new RegisterCommandHandler(
                db,
                Mock.Of<Application.Interfaces.IJwtService>(),
                Mock.Of<Application.Interfaces.IPasswordService>());

            var command = new RegisterCommand(new RegisterModel
            {
                Email = "test@mail.com",
                Password = "123",
                FullName = "Test"
            });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Email already registered", result.Error);
        }
    }
}