namespace Application.DTOs.ResponseModel.Auth
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Error { get; set; }
    }
}
