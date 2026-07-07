using Application.DTOs.ResponseModel.Payment;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<List<CardResponse>> GetCardsAsync(string customerId);
        Task<List<PaymentHistoryResponse>> GetHistoryAsync(string customerId);
    }
}
