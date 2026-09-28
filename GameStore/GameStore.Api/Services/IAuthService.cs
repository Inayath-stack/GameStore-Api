using GameStore.Api.Dtos;

namespace GameStore.Api.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto> RefreshAsync(RefreshTokenRequestDto dto);
    Task DeleteExpiredRefreshTokensAsync();
}