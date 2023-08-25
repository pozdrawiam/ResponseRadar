using Rr.Core.HttpMonitors;
using Rr.Core.Services;

namespace Rr.Web.Services;

public class MonitorWorker : BackgroundService
{
    private readonly TimeSpan _interval;
    private readonly IServiceScopeFactory _scopeFactory;

    public MonitorWorker(IAppConfig config, IServiceScopeFactory scopeFactory)
    {
        _interval = TimeSpan.FromMinutes(config.IntervalMinutes);
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            MonitorService monitorService = scope.ServiceProvider.GetRequiredService<MonitorService>();
            
            await monitorService.CheckUrlsAsync();
            await Task.Delay(_interval, stoppingToken);
        }
    }
}
