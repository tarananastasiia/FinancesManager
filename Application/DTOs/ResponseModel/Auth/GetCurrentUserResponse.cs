namespace Application.DTOs.ResponseModel.Auth
{
    public class GetCurrentUserResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
