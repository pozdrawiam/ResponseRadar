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
            var response = await client.GetAsync(target.Url);

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
