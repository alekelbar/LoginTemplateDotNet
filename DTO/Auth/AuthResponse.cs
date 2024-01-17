namespace webApi.DTO
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime ExpiresIn { get; set; }
    }
}