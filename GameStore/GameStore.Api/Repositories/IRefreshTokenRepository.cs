using System.Security.Principal;
using GameStore.Api.Models;

namespace GameStore.Api.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task UpdateAsync(RefreshToken refreshToken);
    Task DeleteExpiredRefreshTokensAsync();
}