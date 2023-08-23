using System.Net;
using Microsoft.Extensions.Logging;

namespace Rr.Core.HttpMonitors;

public class MonitorService
{
    private readonly IDb _db;
    private readonly ILogger<MonitorService> _logger;

    public MonitorService(IDb db, ILogger<MonitorService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task CheckUrls()
    {
        var client = new HttpClient();
        List<HttpMonitor> targets = _db.HttpMonitors.ToList();

        foreach (var target in targets)
        {
            var response = await client.GetAsync(target.Url);

            if (response.StatusCode == HttpStatusCode.OK)
                _logger.LogInformation("Monitor '{}' ok", target.Name);
            else
                _logger.LogWarning("Monitor '{}' failed with status {}", target.Name, response.StatusCode.ToString());
        }

        await _db.SaveChangesAsync();
    }
}
