using Microsoft.EntityFrameworkCore;
using Rr.Core.HttpMonitors;

namespace Rr.Core;

public interface IDb
{
    DbSet<HttpMonitor> HttpMonitors { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class Db : DbContext, IDb
{
    public DbSet<HttpMonitor> HttpMonitors { get; set; } = null!;
}
