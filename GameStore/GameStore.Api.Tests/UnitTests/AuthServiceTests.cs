using Moq;
using GameStore.Api.Repositories;
using GameStore.Api.Services;
using GameStore.Api.Models;
using GameStore.Api.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GameStore.Api.Exceptions;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
namespace GameStore.Api.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsTokens_AndSavesRefreshToken()
    {
        var userRepositorymock=new Mock<IUserRepository>();
        var passwordHasher=new PasswordHasher<User>();
        var jwtSettings=new JwtSettings
        {
            Key = "ThisIsMySuperSecretKey123456789012345",
            Issuer = "GameStore",
            Audience = "GameStoreUsers"
        };
        var jwtOptions=Options.Create(jwtSettings);
        var refreshTokenRepositorymock=new Mock<IRefreshTokenRepository>();
        var dbOptions=new DbContextOptionsBuilder<GameDbContext>().UseInMemoryDatabase("TestDb").Options;
        var dbContext=new GameDbContext(dbOptions);

        var loggerMock=new Mock<ILogger<AuthService>>();
        var authService=CreateAuthService(
            userRepositorymock,
            passwordHasher,
            jwtOptions,
            refreshTokenRepositorymock,
            dbContext,
            loggerMock);
        var hashedPassword=passwordHasher.HashPassword(new User(),"password123");

        var user=new User
        {
            Id=1,
            Username="John",
            Email="john@gmail.com",
            PasswordHash=hashedPassword,
            Role="User"
        };

        userRepositorymock
            .Setup(x=>x.GetByEmailAsync(It.IsAny<string>()))    
            .ReturnsAsync(user);
        var loginDto=new LoginDto
        {
          Email="john@gmail.com",
          Password="password123"  
        };
        var result=await authService.LoginAsync(loginDto);
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);

        refreshTokenRepositorymock.Verify(
            x=>x.AddAsync(It.IsAny<RefreshToken>()),
            Times.Once
        );
    
    }
    [Fact]
    public async Task LoginAsync_WithEmailDoesNotExist_ThrowsInvalidCredentialsException()
    {
        var userRepositorymock=new Mock<IUserRepository>();
        var refreshTokenRepositorymock=new Mock<IRefreshTokenRepository>();
        var jwtSettings=new JwtSettings
        {
            Key="ThisIsMySuperSecretKey123456789012345",
            Issuer="GameStore",
            Audience="GamestoreUsers"
        };
        var jwtOptions=Options.Create(jwtSettings);
        var passwordHasher=new PasswordHasher<User>();
        var dbOptions=new DbContextOptionsBuilder<GameDbContext>()
                            .UseInMemoryDatabase("TestDb")
                            .Options;
        var dbContext=new GameDbContext(dbOptions);
        var loggerMock=new Mock<ILogger<AuthService>>();
        var authService=CreateAuthService(
            userRepositorymock,
            passwordHasher,
            jwtOptions,
            refreshTokenRepositorymock,
            dbContext,
            loggerMock);

        
        var loginDto=new LoginDto
        {
            Email="abc@gamil.com",
            Password="password123"
        };
        
        userRepositorymock
            .Setup(x=>x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            ()=>authService.LoginAsync(loginDto)
        );
    }
    [Fact]
    public async Task LoginAsync_WithWrongPassword_ThrowsInvalidCredentialsException()
    {
        var userRepositorymock=new Mock<IUserRepository>();
        var refreshTokenRepositorymock=new Mock<IRefreshTokenRepository>();
        var jwtSettings=new JwtSettings
        {
           Key = "ThisIsMySuperSecretKey123456789012345",
            Issuer = "GameStore",
            Audience = "GameStoreUsers" 
        };
        var jwtOptions=Options.Create(jwtSettings);
        var passwordHasher=new PasswordHasher<User>();
        var dbOptions=new DbContextOptionsBuilder<GameDbContext>().UseInMemoryDatabase("TestDb").Options;
        var dbContext=new GameDbContext(dbOptions);
        var loggerMock=new Mock<ILogger<AuthService>>();
        var authService=CreateAuthService(
            userRepositorymock,
            passwordHasher,
            jwtOptions,
            refreshTokenRepositorymock,
            dbContext,
            loggerMock);

        var hashedPassword=passwordHasher.HashPassword(new User(),"password123");
        var user =new User
        {
            Id=1,
            Username="John",
            Email="john@gmail.com",
            PasswordHash=hashedPassword,
            Role="User"
        };
        userRepositorymock
        .Setup(x=>x.GetByEmailAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        var loginDto=new LoginDto
        {
            Email="john@gmail.com",
            Password="password@123"
        };
        
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            ()=>authService.LoginAsync(loginDto)
        );
    }
    private AuthService CreateAuthService(
        Mock<IUserRepository> userRepositorymock,
        PasswordHasher<User> passwordHasher,
        IOptions<JwtSettings> jwtOptions,
        Mock<IRefreshTokenRepository> refreshTokenRepositorymock,
        GameDbContext dbContext,
        Mock<ILogger<AuthService>> loggerMock)
    {
        return new AuthService(
            userRepositorymock.Object,
            passwordHasher,
            jwtOptions,
            refreshTokenRepositorymock.Object,
            dbContext,
            loggerMock.Object);
    }
}


