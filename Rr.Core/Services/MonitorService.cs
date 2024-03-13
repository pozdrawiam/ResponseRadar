using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Logging;
using Rr.Core.Data;

namespace Rr.Core.Services;

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

    public async Task CheckUrlAsync(int id)
    {
        HttpMonitor? monitor = await _db.HttpMonitors.FindAsync(id);
        
        if (monitor != null)
            await CheckUrlAsync(monitor);
    }

    public async Task CheckUrlsAsync()
    {
        HttpMonitor[] monitors = _db.HttpMonitors.ToArray();

        foreach (HttpMonitor monitor in monitors.Where(x => x.IsEnabled))
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
                _logger.LogWarning(e, "Monitor '{Name}' request failed", monitor.Name);
            else
                _logger.LogError(e, "Monitor '{Name}' request failed", monitor.Name);
                
            await _notificationService.NotifyAsync(
                "Monitor '{0}' failed with {1}", monitor.Name, e.GetType().Name);
                
            return;
        }
        finally
        {
            await UpdateMonitorAsync(monitor, response?.StatusCode, stopwatch.ElapsedMilliseconds);
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (monitor.TimeoutMs > 0 && stopwatch.ElapsedMilliseconds > monitor.TimeoutMs)
                await _notificationService.NotifyAsync("Monitor '{0}' ok, but long response", monitor.Name);
            
            _logger.LogInformation("Monitor '{Name}' ok", monitor.Name);
        }
        else
        {
            _logger.LogWarning("Monitor '{Name}' failed with status {Status}", monitor.Name, response.StatusCode.ToString());
            await _notificationService.NotifyAsync(
                "Monitor '{0}' failed with status {1} {2}", monitor.Name, (int)response.StatusCode, response.StatusCode.ToString());
        }
    }

    private async Task UpdateMonitorAsync(HttpMonitor monitor, HttpStatusCode? status, long elapsedMs)
    {
        monitor.CheckedAt = DateTime.UtcNow;
        monitor.Status = (int?)status ?? 0;

        if (elapsedMs <= int.MaxValue)
            monitor.ResponseTimeMs = (int)elapsedMs;
                
        _db.AttachModified(monitor);
        await _db.SaveChangesAsync();
    }
}
