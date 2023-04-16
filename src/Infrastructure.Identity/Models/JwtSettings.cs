using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Models
{
    public class JwtSettings
    {
        [JsonPropertyName("secret")]
        public string Secret { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("audience")]
        public string Audience { get; set; }

        [JsonPropertyName("duration")]
        public int DurationInMinutes { get; set; }

        [JsonPropertyName("refreshDuration")]
        public int RefreshDurationInMinutes { get; set; }
    }
}
