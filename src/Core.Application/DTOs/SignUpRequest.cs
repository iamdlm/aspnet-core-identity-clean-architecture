using System.Text.Json.Serialization;

namespace Core.Application.DTOs
{
    public class SignUpRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}