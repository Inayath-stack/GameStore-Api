using GameStore.Api.Models;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.CRUD;

namespace GameStore.Api.Repositories;

public class GameRepository : IGameRepository
{
    private readonly GameDbContext db;
    public GameRepository(GameDbContext db)
    {
        this.db=db;
    }
    public async Task<IEnumerable<Game>> GetAllAsync()
    {
       return await db.Games.ToListAsync();
    }
    public async Task<Game?> GetByIdAsync(int Id)
    {
        return await db.Games.FirstOrDefaultAsync(g=>g.Id==Id);
    }
    public async Task AddAsync(Game game)
    {
        await db.Games.AddAsync(game);
        await db.SaveChangesAsync();
    }
    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
    public async Task DeleteAsync(Game game)
    {
        db.Games.Remove(game);
        await db.SaveChangesAsync();
    }
}