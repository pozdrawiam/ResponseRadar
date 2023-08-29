using Rr.Core.Services;

namespace Rr.Web.Services;

public class MonitorWorker : BackgroundService
{
    private readonly TimeSpan _interval;
    private readonly ILogger<MonitorWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public MonitorWorker(IAppConfig config, ILogger<MonitorWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _interval = TimeSpan.FromMinutes(config.IntervalMinutes);
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                IMonitorService monitorService = scope.ServiceProvider.GetRequiredService<IMonitorService>();

                await monitorService.CheckUrlsAsync();
                await Task.Delay(_interval, stoppingToken);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "MonitorWorker error");
            }
        }
    }
}
