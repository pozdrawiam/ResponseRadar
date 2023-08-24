using System.Net;
using Microsoft.Extensions.Logging;
using Rr.Core.Services;

namespace Rr.Core.HttpMonitors;

public class MonitorService
{
    private readonly IDb _db;
    private readonly ILogger<MonitorService> _logger;
    private readonly INotificationService _notificationService;

    public MonitorService(IDb db, ILogger<MonitorService> logger, INotificationService notificationService)
    {
        _db = db;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task CheckUrlsAsync()
    {
        using var client = new HttpClient();
        List<HttpMonitor> targets = _db.HttpMonitors.ToList();

        foreach (var target in targets)
        {
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync(target.Url);
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e, "Monitor '{}' request failed", target.Name);
                await _notificationService.NotifyAsync("Monitor '{0}' request failed", target.Name);
                continue;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Monitor '{}' request failed", target.Name);
                continue;
            }

            if (response.StatusCode == HttpStatusCode.OK)
                _logger.LogInformation("Monitor '{}' ok", target.Name);
            else
            {
                _logger.LogWarning("Monitor '{}' failed with status {}", target.Name, response.StatusCode.ToString());
                await _notificationService.NotifyAsync(
                    "Monitor '{0}' failed with status {1}", target.Name, response.StatusCode.ToString());
            }
        }
    }
}
