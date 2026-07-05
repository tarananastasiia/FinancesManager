using Application.DTOs.ResponseModel.Payment;
using MediatR;

namespace Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<CreatePaymentResponse>
    {
        public Guid UserId { get; set; }
    }
}
