using Microsoft.EntityFrameworkCore;
using Rr.Core.HttpMonitors;

namespace Rr.Core;

public interface IDb
{
    DbSet<HttpMonitor> HttpMonitors { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class Db : DbContext, IDb
{
    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public DbSet<HttpMonitor> HttpMonitors => Set<HttpMonitor>();
}
