using GameStore.Api.Repositories;
using GameStore.Api.Dtos;
using Microsoft.AspNetCore.Identity;
using GameStore.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using GameStore.Api.Exceptions;
using Microsoft.Extensions.Options;
using GameStore.Api.Configurations;
using System.Security.Cryptography;
using GameStore.Api.Data;
using System.Transactions;
using Org.BouncyCastle.Asn1.Ocsp;



namespace GameStore.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository userRepository;
    private readonly PasswordHasher<User> passwordHasher;
    
    private readonly JwtSettings jwtSettings;
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly GameDbContext db;
    private readonly ILogger<AuthService> logger;
    public AuthService(IUserRepository userRepository,
                        PasswordHasher<User> passwordHasher,
                        IOptions<JwtSettings> jwtOptions,
                        IRefreshTokenRepository refreshTokenRepository,
                        GameDbContext db,
                        ILogger<AuthService> logger)
    {
        this.userRepository=userRepository;
        this.passwordHasher=passwordHasher;
        this.refreshTokenRepository =refreshTokenRepository;
        jwtSettings=jwtOptions.Value;
        this.db=db;
        this.logger=logger;
    }
    private string GenerateRefreshToken()
    {
        var randomBytes=RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
    public async Task RegisterAsync(RegisterDto registerDto)
    {
        var existingUser=await userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            logger.LogWarning("Registration Failed. User with email {Email} already exists.",registerDto.Email);
            throw new UserAlreadyExistsException();
        }
        var hashedPassword=passwordHasher.HashPassword(new User(),registerDto.Password);
        var user=new User()
        {
           Username=registerDto.Username,
           Email=registerDto.Email,
           PasswordHash=hashedPassword,
           Role="User"
        };
        await userRepository.AddAsync(user);
        logger.LogInformation("User registration successfully with email {Email}.",registerDto.Email);
    }
    private string GenerateAccessToken(User user)
    {
        var key=jwtSettings.Key;
        var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role,user.Role)
        };
        var token=new JwtSecurityToken(
            issuer:jwtSettings.Issuer,
            audience:jwtSettings.Audience,
            claims:claims,
            expires:DateTime.UtcNow.AddHours(1),
            signingCredentials:credentials

        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user=await userRepository.GetByEmailAsync(loginDto.Email);
    
        if (user == null)
        {
            logger.LogWarning("Login failed. User with email {Email} was not found.",loginDto.Email);
            throw new InvalidCredentialsException();
        }
        var result= passwordHasher.VerifyHashedPassword(user,user.PasswordHash,loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            logger.LogWarning("Login failed. Invalid password for email {Email}.",loginDto.Email);
            throw new InvalidCredentialsException();
        }

        var accessToken=GenerateAccessToken(user);

        var refreshToken=GenerateRefreshToken();
        var refreshTokenEntity=new RefreshToken{
            Token=refreshToken,
            ExpiresAt=DateTime.UtcNow.AddDays(30),
            UserId=user.Id
        };
        await refreshTokenRepository.AddAsync(refreshTokenEntity);
        logger.LogInformation("User {User.Id} logged in successfully.",user.Id);
        return new LoginResponseDto
        {
            AccessToken=accessToken,
            RefreshToken=refreshToken
        };
    }
    public async Task<LoginResponseDto> RefreshAsync(RefreshTokenRequestDto dto)
    {
        var storedToken=await refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);
        if (storedToken == null)
        {
            logger.LogWarning("Refresh token was not found.");
            throw new InvalidRefreshTokenException();
        }
        if (storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            logger.LogWarning("Expired refresh token used for user {User.Id}.",storedToken.UserId);
            throw new InvalidRefreshTokenException();
        }
        if(storedToken.RevokedAt != null)
        {
            logger.LogWarning("RevokedInfo refresh token used for user {User.Id}.",storedToken.UserId);
            throw new InvalidRefreshTokenException();
        }
        var accessToken=GenerateAccessToken(storedToken.User);
        var newRefreshToken=GenerateRefreshToken();
        var newRefreshtokenEntity=new RefreshToken
        {
            Token=newRefreshToken,
            ExpiresAt=DateTime.UtcNow.AddDays(30),
            UserId=storedToken.UserId
        };
        using var transaction=await db.Database.BeginTransactionAsync();
        try{
            storedToken.RevokedAt=DateTime.UtcNow;
            await refreshTokenRepository.UpdateAsync(storedToken);

            await refreshTokenRepository.AddAsync(newRefreshtokenEntity);
            await transaction.CommitAsync();
            logger.LogInformation("Refresh token rotated successsfully for user {User.Id}.",storedToken.UserId);
        }
        catch(Exception ex)
        {
            logger.LogError(ex,"Failed to roatate the refresh token for user {User.Id}.",storedToken.UserId);
            await transaction.RollbackAsync();
            throw;
        }

        return new LoginResponseDto
        {
            AccessToken=accessToken,
            RefreshToken=newRefreshToken
        };

    }
    public async Task DeleteExpiredRefreshTokensAsync()
    {
        await refreshTokenRepository.DeleteExpiredRefreshTokensAsync();
    }
 }