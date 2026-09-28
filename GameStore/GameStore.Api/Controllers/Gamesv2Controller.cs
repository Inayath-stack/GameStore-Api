using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/games")]
public class GamesV2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult GetGames()
    {
        return Ok(new
        {
            Message = "Games API Version 2"
        });
    }
}