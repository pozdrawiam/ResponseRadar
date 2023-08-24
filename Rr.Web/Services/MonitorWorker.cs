using Rr.Core.HttpMonitors;

namespace Rr.Web.Services;

public class MonitorWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MonitorWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            MonitorService monitorService = scope.ServiceProvider.GetRequiredService<MonitorService>();
            
            await monitorService.CheckUrlsAsync();
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
