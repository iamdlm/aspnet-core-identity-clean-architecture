namespace Core.Application.DTOs
{
    public class TokenResponse : AuthenticationResponse
    {
        public string? UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
