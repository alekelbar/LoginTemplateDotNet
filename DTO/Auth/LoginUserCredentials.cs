using System.ComponentModel.DataAnnotations;

namespace webApi.DTO
{
    public class LoginUserCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}