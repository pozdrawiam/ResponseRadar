using Microsoft.EntityFrameworkCore;

namespace Rr.Tests;

public static class TestHelper
{
    public static DbSet<T> MockDbSet<T>(IEnumerable<T> list) where T : class
    {
        IQueryable<T> queryableList = list.AsQueryable();
        DbSet<T> dbSet = Substitute.For<DbSet<T>, IQueryable<T>>();

        ((IQueryable<T>)dbSet).Provider.Returns(queryableList.Provider);
        ((IQueryable<T>)dbSet).Expression.Returns(queryableList.Expression);
        ((IQueryable<T>)dbSet).ElementType.Returns(queryableList.ElementType);
        ((IQueryable<T>)dbSet).GetEnumerator().Returns(queryableList.GetEnumerator());

        return dbSet;
    }
}
