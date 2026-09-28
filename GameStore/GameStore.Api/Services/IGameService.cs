using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Org.BouncyCastle.Crypto.Operators;

namespace GameStore.Api.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllAsync();
    Task<GameDto> CreateAsync(CreateGameDto gameDto);
    Task<GameDto?> GetByIdAsync(int Id);
    Task<GameDto?> UpdateAsync(int Id,UpdateGameDto gameDto);
    Task<bool> DeleteAsync(int Id); 


}
