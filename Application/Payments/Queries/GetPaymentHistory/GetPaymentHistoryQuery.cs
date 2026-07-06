using Application.DTOs.ResponseModel.Payment;
using MediatR;

namespace Application.Payments.Queries.GetPaymentHistory
{
    public class GetPaymentHistoryQuery : IRequest<List<PaymentHistoryResponse>>
    {
        public Guid UserId { get; set; }
    }
}
