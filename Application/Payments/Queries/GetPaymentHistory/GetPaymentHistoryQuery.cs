using Application.DTOs.ResponseModel.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Payments.Queries.GetPaymentHistory
{
    public class GetPaymentHistoryQuery : IRequest<List<PaymentHistoryResponse>>
    {
        public Guid UserId { get; set; }
    }
}
