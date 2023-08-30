using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Logging;
using Rr.Core.Data;

namespace Rr.Core.Services;

public interface IMonitorService
{
    Task CheckUrlsAsync();
}

public class MonitorService : IMonitorService
{
    private readonly IDb _db;
    private readonly IHttpService _httpService;
    private readonly ILogger<MonitorService> _logger;
    private readonly INotificationService _notificationService;

    public MonitorService(
        IAppConfig config,
        IDb db, 
        IHttpService httpService,
        ILogger<MonitorService> logger, 
        INotificationService notificationService)
    {
        _db = db;
        _httpService = httpService;
        _httpService.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task CheckUrlsAsync()
    {
        HttpMonitor[] monitors = _db.HttpMonitors.ToArray();

        foreach (HttpMonitor monitor in monitors.Where(x => !string.IsNullOrWhiteSpace(x.Url)))
        {
            await CheckUrlAsync(monitor);
        }
    }

    private async Task CheckUrlAsync(HttpMonitor monitor)
    {
        HttpResponseMessage? response = null;
        Stopwatch stopwatch = Stopwatch.StartNew();
            
        try
        {
            response = await _httpService.GetAsync(monitor.Url);
            stopwatch.Stop();
        }
        catch (Exception e)
        {
            if (e is HttpRequestException)
                _logger.LogWarning(e, "Monitor '{}' request failed", monitor.Name);
            else
                _logger.LogError(e, "Monitor '{}' request failed", monitor.Name);
                
            await _notificationService.NotifyAsync(
                "Monitor '{0}' failed with {1}", monitor.Name, e.GetType().Name);
                
            return;
        }
        finally
        {
            await UpdateMonitorAsync(monitor, response?.StatusCode, stopwatch.ElapsedMilliseconds);
        }
            
        if (response.StatusCode == HttpStatusCode.OK)
            _logger.LogInformation("Monitor '{}' ok", monitor.Name);
        else
        {
            _logger.LogWarning("Monitor '{}' failed with status {}", monitor.Name, response.StatusCode.ToString());
            await _notificationService.NotifyAsync(
                "Monitor '{0}' failed with status {1} {2}", monitor.Name, (int)response.StatusCode, response.StatusCode.ToString());
        }
    }

    private async Task UpdateMonitorAsync(HttpMonitor monitor, HttpStatusCode? status, long elapsedMs)
    {
        monitor.CheckedAt = DateTime.UtcNow;
        monitor.Status = (int?)status ?? 0;
        monitor.ResponseTimeMs = elapsedMs <= int.MaxValue ? (int)elapsedMs : 0;
                
        _db.AttachModified(monitor);
        await _db.SaveChangesAsync();
    }
}
