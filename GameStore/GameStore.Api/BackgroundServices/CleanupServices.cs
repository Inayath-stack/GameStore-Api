using GameStore.Api.Services;
using Microsoft.OpenApi;

namespace GameStore.Api.BackgoundServices;
public class CleanupService : BackgroundService
{
    private readonly ILogger<CleanupService> logger;
    private readonly IServiceScopeFactory scopeFactory;
    public CleanupService(IServiceScopeFactory scopeFactory,ILogger<CleanupService> logger)
    {
        this.scopeFactory=scopeFactory;
        this.logger=logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Cleanup Service started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
            using var Scope=scopeFactory.CreateScope();
            var authService=Scope.ServiceProvider.GetRequiredService<IAuthService>();
            await authService.DeleteExpiredRefreshTokensAsync();
            await Task.Delay(TimeSpan.FromHours(10),stoppingToken);
            }
            catch (OperationCanceledException)
            {
                //expected during shutdown
            }
            catch(Exception ex)
            {
                logger.LogError(ex,"Failed to delete expired refresh tokens");
            }
        }
        logger.LogInformation("Cleanup Service stopped.");
    }
}