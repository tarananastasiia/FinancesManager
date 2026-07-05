using Application.DTOs.ResponseModel.Payment;
using MediatR;
using Stripe;

namespace Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler
        : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
    {
        public async Task<CreatePaymentResponse> Handle(
            CreatePaymentCommand request,
            CancellationToken cancellationToken)
        {
            var service = new PaymentIntentService();

            var intent = await service.CreateAsync(
                new PaymentIntentCreateOptions
                {
                    Amount = 2000,
                    Currency = "usd",
                    AutomaticPaymentMethods = new()
                    {
                        Enabled = true
                    }
                },
                cancellationToken: cancellationToken);

            return new CreatePaymentResponse
            {
                ClientSecret = intent.ClientSecret
            };
        }
    }
}
