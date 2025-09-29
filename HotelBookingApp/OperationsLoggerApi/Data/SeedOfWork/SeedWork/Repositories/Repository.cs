using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OperationsLoggerApi.Data.Models;
using OperationsLoggerApi.Data.SeedOfWork.SeedWork.RepositoriesInterfaces;

namespace OperationsLoggerApi.Data.SeedOfWork.SeedWork.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly OpsLogDbContext _opsLogDbContext;
    protected readonly DbSet<TEntity> _set;

    public Repository(OpsLogDbContext opsLogDbContext)
    {
        _opsLogDbContext = opsLogDbContext;
        _set = opsLogDbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Query() => _set.AsQueryable();
    public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _set.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return query;
    }

    public async Task<TEntity?> GetByIdAsync(object id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct).AsTask();

    public Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>>[]? includes = default)
    {
        IQueryable<TEntity> query = _set;

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.FirstOrDefaultAsync(predicate);
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
        => _set.FirstOrDefaultAsync(predicate, ct);

    public Task<TEntity?> FindAsync(int id, CancellationToken ct = default)
    {
        return _set.FindAsync(new object[] { id }, ct).AsTask();
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.Where(predicate);
    }

    public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        => predicate is null
            ? await _set.ToListAsync(ct)
            : await _set.Where(predicate).ToListAsync(ct);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => _set.AnyAsync(predicate, ct);

    public Task AddAsync(TEntity entity, CancellationToken ct = default)
        => _set.AddAsync(entity, ct).AsTask();

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        => _set.AddRangeAsync(entities, ct);

    public void Update(TEntity entity) => _set.Update(entity);

    public void Remove(TEntity entity) => _set.Remove(entity);
    public Task RemoveAsync(TEntity entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }

    public void RemoveRange(IEnumerable<TEntity> entities) => _set.RemoveRange(entities);
}