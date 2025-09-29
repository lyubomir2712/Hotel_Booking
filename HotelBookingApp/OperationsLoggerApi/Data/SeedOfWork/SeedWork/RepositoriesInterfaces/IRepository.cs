using System.Linq.Expressions;

namespace OperationsLoggerApi.Data.SeedOfWork.SeedWork.RepositoriesInterfaces;
public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Query();

    IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetByIdAsync(object id, CancellationToken ct = default);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default);

    Task<TEntity?> FindAsync(int id, CancellationToken ct = default);
    
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

    Task AddAsync(TEntity entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);

    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task RemoveAsync(TEntity entity, CancellationToken ct = default);
    void RemoveRange(IEnumerable<TEntity> entities);
}
