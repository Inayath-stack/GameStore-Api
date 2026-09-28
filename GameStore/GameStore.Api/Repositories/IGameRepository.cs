using GameStore.Api.Models;
namespace GameStore.Api.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(int Id);
    Task AddAsync(Game game);
    Task SaveChangesAsync();
    Task DeleteAsync(Game game);

  
}