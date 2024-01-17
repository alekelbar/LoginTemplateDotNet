using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webApi.DTO;
using webApi.Entities;

namespace webApi.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration configuration
        ) : ControllerBase
    {
        private readonly UserManager<AppUser> userManager = userManager;
        private readonly SignInManager<AppUser> signInManager = signInManager;
        private readonly IConfiguration configuration = configuration;

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(
            RegisterUserCredentials credentials
        )
        {
            var user = await userManager.CreateAsync(
                new AppUser()
                {
                    UserName = credentials.Email,
                    Email = credentials.Email,
                    FirtsName = credentials.FirtsName,
                    LastName = credentials.LastName
                },
                credentials.Password
            );

            if (user.Succeeded)
                return GetToken(userCredentials: credentials);
            else
                return BadRequest(user.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserCredentials credentials)
        {
            var user = await signInManager.PasswordSignInAsync(
                credentials.Email,
                credentials.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if(user.Succeeded) return GetToken(userCredentials: credentials);
            else return BadRequest("login failed");
        }

        [HttpGet("refresh")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<AuthResponse> RefreshToken()
        {
            var userEmailClaim = HttpContext.User.Claims.Where(x => x.Type == "Email").FirstOrDefault();
            if (userEmailClaim == null) return NotFound("User email not found");

            var userEmail = userEmailClaim.Value;
            var userCredentials = new LoginUserCredentials()
            {
                Email = userEmail
            };
            return GetToken(userCredentials);
        }


        private AuthResponse GetToken(LoginUserCredentials userCredentials)
        {
            var claims = new List<Claim>(){
                new("email", userCredentials.Email)
            };

            var expires = DateTime.Now.AddDays(1);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(issuer: null,
                audience: null,
                claims: claims,
                expires: expires,
                signingCredentials: creds
               );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Email = userCredentials.Email,
                ExpiresIn = expires
            };
        }
        
    }
}