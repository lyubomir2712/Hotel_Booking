using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.SeedWork.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly BookingDbContext _bookingDbContext;
    protected readonly DbSet<TEntity> _set;

    public Repository(BookingDbContext bookingDbContext)
    {
        _bookingDbContext = bookingDbContext;
        _set = bookingDbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Query() => _set.AsQueryable();

    public async Task<TEntity?> GetByIdAsync(object id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct).AsTask();

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => _set.FirstOrDefaultAsync(predicate, ct);

    public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => _set.FirstAsync(predicate, ct);

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

    public void RemoveRange(IEnumerable<TEntity> entities) => _set.RemoveRange(entities);
}