using HotelBooking.Data.SeedWork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.SeedWork;


public class UnitOfWork : IUnitOfWork
{
    private readonly BookingDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(BookingDbContext context)
    {
        _context = context;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (_repositories.TryGetValue(type, out var repo))
            return (IRepository<TEntity>)repo;

        var newRepo = new Repository<TEntity>(_context);
        _repositories[type] = newRepo;
        return newRepo;
    }

    public int SaveChanges() => _context.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}