using System.Net.Http.Json;
using GameStore.Api.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GameStore.Api.Tests.IntegrationTests;
public class AuthControllerIntegrationTests:IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient client;
    public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        client=factory.CreateClient();
    }
    [Fact]
    public async Task LoginAsync_WithValidCredentials_Gives_AccessTokenAndRefreshToken()
    {
        var registerDto=new RegisterDto
        {
            Username="John2",
            Email="John2@gmail.com",
            Password="password@123"
        };
        var registerResponse=await client.PostAsJsonAsync("/api/auth/register",registerDto);
        
        var responseBody = await registerResponse.Content.ReadAsStringAsync();

        Console.WriteLine(registerResponse.StatusCode);
        Console.WriteLine(responseBody);
        Assert.True(registerResponse.IsSuccessStatusCode);

        var loginDto=new LoginDto
        {
            Email="John2@gmail.com",
            Password="password@123"
        };
        var loginResponse=await client.PostAsJsonAsync("/api/auth/login",loginDto);
        var response=await loginResponse.Content.ReadAsStringAsync();
        Console.WriteLine(response);
        Console.WriteLine(loginResponse.StatusCode);
        Assert.True(loginResponse.IsSuccessStatusCode);
        var loginResult=await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.NotNull(loginResult);
    }
}