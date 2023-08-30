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
            HttpResponseMessage? response = null;
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            try
            {
                response = await _httpService.GetAsync(monitor.Url);
                stopwatch.Stop();
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Monitor '{}' request failed", monitor.Name);
                await _notificationService.NotifyAsync("Monitor '{0}' request failed", monitor.Name);
                continue;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Monitor '{}' request failed", monitor.Name);
                continue;
            }
            finally
            {
                monitor.CheckedAt = DateTime.UtcNow;
                monitor.Status = (int?)(response?.StatusCode) ?? 0;
                monitor.ResponseTimeMs = stopwatch.ElapsedMilliseconds <= int.MaxValue ? (int)stopwatch.ElapsedMilliseconds : 0;
                
                _db.AttachModified(monitor);
                await _db.SaveChangesAsync();
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
    }
}
