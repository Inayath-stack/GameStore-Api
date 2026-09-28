using Microsoft.EntityFrameworkCore;
using GameStore.Api.Models;

namespace GameStore.Api.Data;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) //adds DbContext creates DbContextOptions passes them into GameDbContext and it later passes into DbContext
        : base(options)
    {
    }
    public DbSet<Game> Games {get;set;}=null!;
    public DbSet<User> Users{get;set;}=null!;
    public DbSet<RefreshToken> RefreshTokens{get;set;}=null!;
}
