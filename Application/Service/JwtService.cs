using Application.Common.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Application.Service
{
    public class JwtService : IJwtService
    {

        // JWT generation
        public (string, DateTime) GenerateJwtToken(User user, string role, JwtSettings jwtSettings)
        {
            if (string.IsNullOrEmpty(jwtSettings.SecretKey))
                throw new InvalidOperationException("JWT SecretKey is not configured");
            if (string.IsNullOrEmpty(jwtSettings.Issuer))
                throw new InvalidOperationException("JWT Issuer is not configured");
            if (string.IsNullOrEmpty(jwtSettings.Audience))
                throw new InvalidOperationException("JWT Audience is not configured");
            
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("user_id", user.Id.ToString())
            };
            
            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );
            return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }

        // Generate refresh token
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // Get principal from expired token
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, JwtSettings jwtSettings)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidateLifetime = false, 
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                
                // Validate the token structure first
                if (!tokenHandler.CanReadToken(token))
                {
                    throw new SecurityTokenException("Token format is invalid");
                }

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                
                if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm");
                }

                return principal;
            }
            catch (Exception)
            {
                // Re-throw the exception to be handled by the calling method
                throw;
            }
        }
    }
}
