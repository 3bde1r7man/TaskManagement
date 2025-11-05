using Application.Common.Results;
using Application.DTOs;

namespace Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthTokenData>> RegisterUserAsync(UserRegisterRequest request);
        Task<ServiceResult<AuthTokenData>> LoginUserAsync(LoginRequest request);
        Task<ServiceResult<bool>> LogoutAsync();
        Task<ServiceResult<AuthTokenData>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
