using GameStore.Api.Dtos;
using GameStore.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Api.Tests;

public class AuthControllerTests
{
    [Fact]
    public async Task LoginWithValidCredentials_ReturnsOk()
    {
        var authServicemock=new Mock<IAuthService>();

        var controller=new AuthController(authServicemock.Object);
        var loginResponse=new LoginResponseDto
        {
            AccessToken="abc",
            RefreshToken="xyz"
        };
        authServicemock
        .Setup(x=>x.LoginAsync(It.IsAny<LoginDto>()))
        .ReturnsAsync(loginResponse);

        var loginDto=new LoginDto
        {
            Email="abc@gmail.com",
            Password="password123"
        };

        var result=await controller.Login(loginDto);
        var okResult=Assert.IsType<OkObjectResult>(result);

        var response=Assert.IsType<LoginResponseDto>(okResult.Value);

        Assert.Equal("abc",response.AccessToken);
        Assert.Equal("xyz",response.RefreshToken);

        authServicemock.Verify(
            x=>x.LoginAsync(It.IsAny<LoginDto>()),
            Times.Once  
        );
    }
}