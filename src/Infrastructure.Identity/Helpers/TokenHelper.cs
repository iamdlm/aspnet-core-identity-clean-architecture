using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity.Helpers
{
    public static class TokenHelper
    {
        public static List<Claim> GenerateClaims(string userId, string userName, string userRole)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, userRole)
            };
        }

        public static string GenerateAuthToken(IEnumerable<Claim> claims, string jwtSecret, int jwtTokenDuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtTokenDuration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var authToken = tokenHandler.WriteToken(token);

            return authToken;
        }

        public static string GenerateRefreshToken()
        {
            // Define the length of the refresh token
            int refreshTokenLength = 32; // or any desired length

            // Generate a byte array to hold the random bytes
            byte[] randomBytes = new byte[refreshTokenLength];

            // Create a new instance of the random number generator
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert the random bytes to a string representation
            string refreshToken = Convert.ToBase64String(randomBytes);

            return refreshToken;
        }

        public static  ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string jwtSecret)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, // you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateLifetime = false // here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            
            return principal;
        }
    }
}
