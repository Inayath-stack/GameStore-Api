using GameStore.Api.Models;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crmf;

namespace GameStore.Api.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly GameDbContext db;
    public RefreshTokenRepository(GameDbContext db)
    {
        this.db=db;
    }
    public async Task AddAsync(RefreshToken refreshToken)
    {
        await db.RefreshTokens.AddAsync(refreshToken);
        await db.SaveChangesAsync();
    }
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await db.RefreshTokens.Include(x=>x.User).FirstOrDefaultAsync(x=>x.Token==token);
    }
    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        db.RefreshTokens.Update(refreshToken);
        await db.SaveChangesAsync();
    }
    public async Task DeleteExpiredRefreshTokensAsync()
    {
        var expiredTokens=await db.RefreshTokens.Where(rt=>rt.ExpiresAt<DateTime.UtcNow).ToListAsync();
        db.RemoveRange(expiredTokens);
        await db.SaveChangesAsync();
    }
}