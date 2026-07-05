using Infrastructure.Services;
using Moq;
using Stripe;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using IStripeClient = Infrastructure.Services.IStripeClient;

namespace Tests.Payment
{
    [TestClass]
    public class PaymentTests
    {
        [TestMethod]
        public async Task GetCards_Should_Return_Cards()
        {
            var stripeMock = new Mock<IStripeClient>();

            stripeMock.Setup(x => x.GetCardMethods("cus_123"))
                .ReturnsAsync(new List<PaymentMethod>
                {
                    new PaymentMethod
                    {
                        Card = new PaymentMethodCard
                        {
                            Brand = "visa",
                            Last4 = "4242",
                            ExpMonth = 12,
                            ExpYear = 2030
                        }
                    }
                });

            var service = new PaymentService(stripeMock.Object);

            var result = await service.GetCards("cus_123");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("visa", result[0].Brand);
            Assert.AreEqual("4242", result[0].Last4);
        }

        [TestMethod]
        public async Task GetHistory_Should_Return_Payments()
        {
            var fixedDate = new DateTime(2023, 11, 14, 0, 0, 0, DateTimeKind.Utc);

            var stripeMock = new Mock<IStripeClient>();

            stripeMock.Setup(x => x.GetPaymentHistory("cus_123", 20))
                .ReturnsAsync(new List<PaymentIntent>
                {
                    new PaymentIntent
                    {
                        Id = "pi_1",
                        Amount = 1000,
                        Currency = "usd",
                        Status = "succeeded",

                        Created = fixedDate
                    }
                });

            var service = new PaymentService(stripeMock.Object);

            var result = await service.GetHistory("cus_123");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("pi_1", result[0].Id);
            Assert.AreEqual(10.0, result[0].Amount);
            Assert.AreEqual("usd", result[0].Currency);
            Assert.AreEqual("succeeded", result[0].Status);

            Assert.AreEqual(
                DateTimeOffset.FromUnixTimeSeconds(fixedDate.Second).UtcDateTime,
                result[0].Date
            );
        }
    }
}