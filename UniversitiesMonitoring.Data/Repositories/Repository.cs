using Microsoft.EntityFrameworkCore;

namespace UniversityMonitoring.Data.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public Task AddAsync(TEntity entity) => _dbSet.AddAsync(entity).AsTask();

    public Task<TEntity?> FindAsync(TKey key) => _dbSet.FindAsync(key).AsTask();

    public void Remove(TEntity entity) => _dbSet.Remove(entity);
    public IEnumerable<TEntity> ExecuteSql(string sql) => _dbSet.FromSqlRaw(sql);
    public IEnumerable<TEntity> GetlAll() => _dbSet;
}