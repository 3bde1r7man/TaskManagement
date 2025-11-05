using Domain.Entities;
using Application.Models;
using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface IJwtService
    {
        (string, DateTime) GenerateJwtToken(User user, string role, JwtSettings jwtSettings);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, JwtSettings jwtSettings);
    }
}
