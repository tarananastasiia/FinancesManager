using Application.DTOs.ResponseModel.Payment;
using Application.Interfaces;
using MediatR;
using Stripe;

namespace Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
    {
        private readonly IStripeService _stripeService;

        public CreatePaymentCommandHandler(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        public async Task<CreatePaymentResponse> Handle(
            CreatePaymentCommand request,
            CancellationToken cancellationToken)
        {
            var intent = await _stripeService.CreatePaymentIntentAsync(
            new PaymentIntentCreateOptions
            {
                Amount = 2000,
                Currency = "usd",
                AutomaticPaymentMethods = new()
                {
                    Enabled = true
                }
            },
            cancellationToken);

            return new CreatePaymentResponse
            {
                ClientSecret = intent.ClientSecret
            };
        }
    }
}
