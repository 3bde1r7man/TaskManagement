using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class UserRegisterRequest
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        [RegularExpression(@"^01([0-2]|5)\d{8}$", ErrorMessage = "Invalid Egyptian phone number format")]
        public required string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
        
        [Required]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid role specified")]
        public required UserRole Role { get; set; }
    }

    public class RefreshTokenRequest
    {
        [Required]
        public required string Token { get; set; }

        [Required]
        public required string RefreshToken { get; set; }
    }

    // Response DTOs
    public class AuthResponse
    {
        public required string Message { get; set; }
        public int UserId { get; set; }
        public string? UserRole { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string? RefreshToken { get; set; }
        public object? AdditionalData { get; set; }
    }

    public class ErrorResponse
    {
        public required string Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    } 
}