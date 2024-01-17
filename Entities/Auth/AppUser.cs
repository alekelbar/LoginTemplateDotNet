using Microsoft.AspNetCore.Identity;

namespace webApi.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirtsName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}