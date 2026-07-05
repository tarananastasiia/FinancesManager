namespace Application.DTOs.ResponseModel.Payment
{
    public class PaymentHistoryResponse
    {
        public string Id { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Card { get; set; }
    }
}
