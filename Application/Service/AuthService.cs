using Domain.Entities;
using Domain.Constants;
using Application.DTOs;
using Application.Common.Results;
using Application.Models;
using Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, IJwtService jwtService, JwtSettings jwtSettings)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings;
        }

        public async Task<ServiceResult<AuthTokenData>> RegisterUserAsync(UserRegisterRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userRepository.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return ServiceResult<AuthTokenData>.Failure(
                        "User with this email already exists", 
                        "Email already in use");
                }

                // Create User instance
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    UserName = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    Role = request.Role
                };

                var result = await _userRepository.CreateUserAsync(user, request.Password);

                if (result.Succeeded)
                {

                    // Generate Access Token and Refresh Token
                    var token = _jwtService.GenerateJwtToken(user, request.Role.ToString(), _jwtSettings);
                    var refreshToken = _jwtService.GenerateRefreshToken();
                    
                    // Update user with refresh token
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
                    await _userRepository.UpdateUserAsync(user);
                    
                    // Add to RegularUser role
                    await _userRepository.AddToRoleAsync(user, request.Role.ToString());
                    await _userRepository.SaveChangesAsync();

                    var authData = new AuthTokenData
                    {
                        UserId = user.Id,
                        Token = token.Item1,
                        ExpiresIn = token.Item2,
                        UserRole = user.Role.ToString(),
                        RefreshToken = refreshToken
                    };

                    return ServiceResult<AuthTokenData>.Success(authData, "Registration successful");
                }

                return ServiceResult<AuthTokenData>.Failure(
                    "Registration failed", 
                    result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthTokenData>.Failure(
                    "An error occurred during registration", 
                    ex.Message);
            }
        }

        public async Task<ServiceResult<AuthTokenData>> LoginUserAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return ServiceResult<AuthTokenData>.Failure("Invalid login attempt");
                }

                var result = await _userRepository.PasswordSignInAsync(user, request.Password, false, false);

                if (result.Succeeded)
                {
                    // Get user roles from Identity
                    var userRoles = await _userRepository.GetRolesAsync(user);
                    var userRole = userRoles.FirstOrDefault() ?? user.Role.ToString();

                    // Generate Access Token and Refresh Token
                    var token = _jwtService.GenerateJwtToken(user, user.Role.ToString(), _jwtSettings);
                    var refreshToken = _jwtService.GenerateRefreshToken();
                    
                    // Update user with refresh token
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
                    await _userRepository.UpdateUserAsync(user);
                    
                    var authData = new AuthTokenData
                    {
                        UserId = user.Id,
                        Token = token.Item1,
                        ExpiresIn = token.Item2,
                        UserRole = user.Role.ToString(),
                        RefreshToken = refreshToken
                    };

                    return ServiceResult<AuthTokenData>.Success(authData, "Login successful");
                }

                return ServiceResult<AuthTokenData>.Failure("Invalid email or password");
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthTokenData>.Failure("An error occurred during login");
            }
        }

        public async Task<ServiceResult<bool>> LogoutAsync()
        {
            try
            {
                await _userRepository.SignOutAsync();
                return ServiceResult<bool>.Success(true, "Logout successful");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure("Logout failed", ex.Message);
            }
        }

        public async Task<ServiceResult<AuthTokenData>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                ClaimsPrincipal? principal;
                try
                {
                    principal = _jwtService.GetPrincipalFromExpiredToken(request.Token, _jwtSettings);
                }
                catch (Exception ex)
                {
                    return ServiceResult<AuthTokenData>.Failure("Invalid access token", ex.Message);
                }
                
                if (principal is null)
                {
                    return ServiceResult<AuthTokenData>.Failure("Invalid access token - unable to extract claims");
                }

                // Try multiple ways to get the user ID claim
                var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub) 
                    ?? principal.FindFirst(ClaimTypes.NameIdentifier) 
                    ?? principal.FindFirst("sub");
                    
                if (userIdClaim == null)
                {
                    // Debug: List all available claims
                    var availableClaims = string.Join(", ", principal.Claims.Select(c => $"{c.Type}:{c.Value}"));
                    return ServiceResult<AuthTokenData>.Failure($"Invalid token claims - missing user ID. Available claims: {availableClaims}");
                }

                var userId = userIdClaim.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdInt))
                {
                    return ServiceResult<AuthTokenData>.Failure($"Invalid token claims - user ID format invalid: {userId}");
                }

                var user = await _userRepository.FindByIdAsync(userIdInt);
                if (user == null)
                {
                    return ServiceResult<AuthTokenData>.Failure("User not found");
                }

                if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return ServiceResult<AuthTokenData>.Failure("Invalid refresh token or expired");
                }

                // Generate new tokens
                var userRoles = await _userRepository.GetRolesAsync(user);
                var userRole = userRoles.FirstOrDefault() ?? "user";
                
                var newJwtToken = _jwtService.GenerateJwtToken(user, userRole, _jwtSettings);
                var newRefreshToken = _jwtService.GenerateRefreshToken();

                // Update user with new refresh token
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
                await _userRepository.UpdateUserAsync(user);

                var authData = new AuthTokenData
                {
                    UserId = user.Id,
                    Token = newJwtToken.Item1,
                    ExpiresIn = newJwtToken.Item2,
                    UserRole = userRole,
                    RefreshToken = newRefreshToken
                };

                return ServiceResult<AuthTokenData>.Success(authData, "Token refreshed successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthTokenData>.Failure("An error occurred while refreshing token", ex.Message);
            }
        }
    }
}