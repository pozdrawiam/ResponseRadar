using Microsoft.EntityFrameworkCore;

namespace Rr.Core.Data;

public interface IDb
{
    DbSet<HttpMonitor> HttpMonitors { get; }

    void AttachModified(object entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class Db(DbContextOptions<Db> options) 
    : DbContext(options), IDb
{
    public DbSet<HttpMonitor> HttpMonitors => Set<HttpMonitor>();

    public void AttachModified(object entity)
    {
        Attach(entity).State = EntityState.Modified;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HttpMonitor>().Property(p => p.IsEnabled).HasDefaultValue(true);
    }
}
