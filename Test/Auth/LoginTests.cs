using Application.Auth.Commands.Login;
using Application.DTOs.RequestModel;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Auth
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public async Task Login_Should_Return_Token_When_Valid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ApplicationDbContext(options);

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@mail.com",
                FullName = "Test",
                PasswordHash = "hashed"
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            jwtMock.Setup(x => x.GenerateToken(It.IsAny<ApplicationUser>()))
                   .Returns("fake-token");

            var passwordMock = new Mock<IPasswordService>();
            passwordMock.Setup(x => x.Verify(user, "123"))
                         .Returns(true);

            var handler = new LoginCommandHandler(
                jwtMock.Object,
                db,
                passwordMock.Object);

            var command = new LoginCommand(new LoginModel
            {
                Email = "test@mail.com",
                Password = "123"
            });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("fake-token", result.Token);
        }
    }
}