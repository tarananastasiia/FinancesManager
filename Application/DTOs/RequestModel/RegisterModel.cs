using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.RequestModel
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string FullName { get; set; } = "";
    }
}
