using GameStore.Api.Repositories;
using GameStore.Api.Data;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GameDbContext db;
    public UserRepository(GameDbContext db)
    {
        this.db=db;
    }
    public async Task<User?> GetByEmailAsync(string Email)
    {
        return await db.Users.FirstOrDefaultAsync(u=>u.Email==Email);
        
    }
    public async Task AddAsync(User user)
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
    }
}