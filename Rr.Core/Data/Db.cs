using Microsoft.EntityFrameworkCore;

namespace Rr.Core.Data;

public interface IDb
{
    DbSet<HttpMonitor> HttpMonitors { get; }

    void AttachModified(object entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class Db : DbContext, IDb
{
    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public DbSet<HttpMonitor> HttpMonitors => Set<HttpMonitor>();
    
    public void AttachModified(object entity)
    {
        Attach(entity).State = EntityState.Modified;
    }
}
