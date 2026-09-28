using GameStore.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GameStore.Api.Tests;
public class CustomWebApplicationFactory:WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptors = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<GameDbContext>) ||
                    d.ServiceType == typeof(IDbContextOptionsConfiguration<GameDbContext>) ||
                    d.ServiceType == typeof(GameDbContext))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<GameDbContext>(Options =>
            {
                Options.UseInMemoryDatabase("IntegrationTestsDb");
            });

        });
        
    }
}