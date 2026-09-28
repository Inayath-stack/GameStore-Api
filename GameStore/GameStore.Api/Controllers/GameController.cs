using GameStore.Api.Dtos;
using GameStore.Api.Data;
using GameStore.Api.Models;
using GameStore.Api.Repositories;
using GameStore.Api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
// using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers;


[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/games")]
public class GameController : ControllerBase
{
//     private static GameDto ToDto(Game game)
// {
//     return new GameDto
//     {
//         Id=game.Id,
//         Name=game.Name,
//         Genre=game.Genre
//     };
// }
    
    [HttpGet]
    public async Task<IActionResult> GetGames(IGameService service)
    {
        var games =await service.GetAllAsync();

        return Ok(games);  
    }
    [Authorize(Roles="Admin")]
    [HttpPost]                 
    public async Task<IActionResult> CreateGames(CreateGameDto gameDto,IGameService service)
    {
        
        var game=await service.CreateAsync(gameDto);

    
        return Ok(game);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateGames(int Id,UpdateGameDto gameDto,IGameService service)
    {
        var game=await service.UpdateAsync(Id,gameDto);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }
    [Authorize(Roles ="Admin")]
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteGames(int Id,IGameService service)
    {
        var deleted=await service.DeleteAsync(Id);
        if(!deleted){
            return NotFound();
        }
        return NoContent();
        
    }
}