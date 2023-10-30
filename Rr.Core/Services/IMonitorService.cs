namespace Rr.Core.Services;

public interface IMonitorService
{
    Task CheckUrlAsync(int id);
    Task CheckUrlsAsync();
}
