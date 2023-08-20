namespace Rr.Core.HttpMonitors.Create;

public class HttpMonitorCreateHandler
{
    private readonly IDb _db;

    public HttpMonitorCreateHandler(IDb db)
    {
        _db = db;
    }

    public async Task HandleAsync(HttpMonitorCreateCmd cmd)
    {
        //todo

        await _db.SaveChangesAsync();
    }
}
