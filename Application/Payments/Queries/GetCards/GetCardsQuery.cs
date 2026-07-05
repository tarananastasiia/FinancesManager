using Application.DTOs.ResponseModel.Payment;
using MediatR;

namespace Application.Payments.Queries.GetCards
{
    public class GetCardsQuery : IRequest<List<CardResponse>>
    {
        public Guid UserId { get; set; }
    }
}
