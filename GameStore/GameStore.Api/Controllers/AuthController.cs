using System.Runtime.CompilerServices;
using GameStore.Api.Controllers;
using GameStore.Api.Dtos;
using GameStore.Api.Filters;
using GameStore.Api.Models;
using GameStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

[ServiceFilter(typeof(AppVersionResultFilter))]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    public AuthController(IAuthService authService)
    {
        this.authService=authService;
    }
    /// <summary>
    /// Authenticates a user and returns an access token and refresh token.
    /// </summary>
    /// <param name="loginDto">The user's email and password</param>
    /// <returns>Returns the generated access token and response token</returns>
    /// <response code="200">Login was Successful</response>
    /// <response code="401">Invalid email or password</response>
    [ProducesResponseType(typeof(LoginResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto),StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var response=await authService.LoginAsync(loginDto);
        return Ok(response);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        await authService.RegisterAsync(registerDto);
        return Ok(new {message="User registered successfully."});
    }
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshAsync(RefreshTokenRequestDto dto)
    {
        var response=await authService.RefreshAsync(dto);
        return Ok(response);
    }
    
}
