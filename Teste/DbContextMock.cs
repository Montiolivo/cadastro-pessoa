using Microsoft.EntityFrameworkCore;
using Moq;

public static class DbContextMock
{
    public static Mock<DbSet<T>> CreateDbSet<T>(IEnumerable<T> source) where T : class
    {
        var queryable = source.AsQueryable();

        var dbSet = new Mock<DbSet<T>>();
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        return dbSet;
    }

    public static Mock<DbSet<T>> CreateDbSet<T>(IQueryable<T> source) where T : class
    {
        return CreateDbSet(source.ToList());
    }
}
