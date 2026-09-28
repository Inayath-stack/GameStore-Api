using System.Data.Common;
using System.Security.Policy;
using AutoMapper;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Exceptions;
using GameStore.Api.Models;
using GameStore.Api.Repositories;
using GameStore.Api.Services;

namespace GameStore.Api.Services;

public class GameService : IGameService
{
    private readonly IGameRepository repository;
    private readonly IMapper mapper;
    public GameService(IGameRepository repository,IMapper mapper)
    {
        this.repository=repository;
        this.mapper=mapper;
    } 
    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        var games=await repository.GetAllAsync();
        return mapper.Map<IEnumerable<GameDto>>(games);

    }
    public async Task<GameDto> CreateAsync(CreateGameDto gameDto)
    {
        var game=mapper.Map<Game>(gameDto);
        await repository.AddAsync(game);
        return mapper.Map<GameDto>(game);

    }
    public async Task<GameDto?> GetByIdAsync(int Id)
    {
        var game=await repository.GetByIdAsync(Id);
        if (game == null)
        {
            throw new GameNotFoundException();
        }

        return mapper.Map<GameDto>(game);
    }
    public async Task<GameDto?> UpdateAsync(int Id,UpdateGameDto gameDto)
    {
        var game=await repository.GetByIdAsync(Id);
        if (game == null)
        {
            throw new GameNotFoundException();
        }
        mapper.Map(gameDto,game);
        await repository.SaveChangesAsync();

        return mapper.Map<GameDto>(game);
        
    }
    public async Task<bool> DeleteAsync(int Id)
    {
        var game=await repository.GetByIdAsync(Id);
        if (game == null)
        {
            throw new GameNotFoundException();
        }

        await repository.DeleteAsync(game);
        return true;
    }
     
}