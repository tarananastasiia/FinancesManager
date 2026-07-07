using Domain.Enums;

namespace Application.DTOs.ResponseModel.Payment
{
    public class PaymentHistoryResponse
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string Card { get; set; }
        public string Description { get; set; }

        public string Category { get; set; } = string.Empty;
    }
}
