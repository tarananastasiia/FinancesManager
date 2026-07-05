namespace Application.DTOs.ResponseModel.Payment
{
    public class CardResponse
    {
        public string Brand { get; set; } = string.Empty;
        public string Last4 { get; set; } = string.Empty;
        public long? ExpMonth { get; set; }
        public long? ExpYear { get; set; }
    }
}
