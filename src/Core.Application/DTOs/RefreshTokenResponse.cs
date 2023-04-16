namespace Core.Application.DTOs
{
    public class RefreshTokenResponse : AuthenticationResponse
    {
        public string Token { get; set; }
        public string Expiration { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
