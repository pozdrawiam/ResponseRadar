using System.Net;
using Microsoft.Extensions.Logging;
using Rr.Core.Data;

namespace Rr.Core.Services;

//todo interface
public class MonitorService
{
    private readonly TimeSpan _timeout;
    private readonly IDb _db;
    private readonly ILogger<MonitorService> _logger;
    private readonly INotificationService _notificationService;

    public MonitorService(IAppConfig config, IDb db, ILogger<MonitorService> logger, INotificationService notificationService)
    {
        _timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
        _db = db;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task CheckUrlsAsync()
    {
        using var client = new HttpClient(); //todo remove new
        client.Timeout = _timeout;

        HttpMonitor[] monitors = _db.HttpMonitors.ToArray();

        foreach (HttpMonitor monitor in monitors)
        {
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync(monitor.Url);
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
