using Application.Common.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class UserAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _authService.RegisterUserAsync(request);

            if (result.IsSuccess && result.Data != null)
            {
                return Ok(new AuthResponse
                {
                    Message = result.Message,
                    UserId = result.Data.UserId,
                    UserRole = result.Data.UserRole,
                    Token = result.Data.Token,
                    ExpiresIn = result.Data.ExpiresIn,
                    RefreshToken = result.Data.RefreshToken
                });
            }

            return BadRequest(new ErrorResponse
            {
                Message = result.Message,
                Errors = result.Errors
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _authService.LoginUserAsync(request);

            if (result.IsSuccess && result.Data != null)
            {
                return Ok(new AuthResponse
                {
                    Message = result.Message,
                    UserId = result.Data.UserId,
                    UserRole = result.Data.UserRole,
                    Token = result.Data.Token,
                    ExpiresIn = result.Data.ExpiresIn,
                    RefreshToken = result.Data.RefreshToken
                });
            }

            return BadRequest(new ErrorResponse
            {
                Message = result.Message,
                Errors = result.Errors
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _authService.RefreshTokenAsync(request);

            if (result.IsSuccess && result.Data != null)
            {
                return Ok(new AuthResponse
                {
                    Message = result.Message,
                    UserId = result.Data.UserId,
                    UserRole = result.Data.UserRole,
                    Token = result.Data.Token,
                    ExpiresIn = result.Data.ExpiresIn,
                    RefreshToken = result.Data.RefreshToken
                });
            }

            return BadRequest(new ErrorResponse
            {
                Message = result.Message,
                Errors = result.Errors
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();

            if (result.IsSuccess)
            {
                return Ok(new AuthResponse
                {
                    Message = result.Message,
                    UserId = 0
                });
            }

            return BadRequest(new ErrorResponse
            {
                Message = result.Message,
                Errors = result.Errors
            });
        }
    }
}