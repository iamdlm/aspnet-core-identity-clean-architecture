using System.Text.Json.Serialization;

namespace Core.Application.DTOs
{
    public class RefreshTokenRequest
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set;}
        
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set;}
    }
}
