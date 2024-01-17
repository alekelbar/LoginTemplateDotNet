using System.ComponentModel.DataAnnotations;

namespace webApi.DTO
{
    public class RegisterUserCredentials : LoginUserCredentials
    {
        [Required]
        public string FirtsName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
    }
}