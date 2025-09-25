using HotelBooking.Data.SeedWork.Repositories;
using HotelBooking.Data.SeedWork.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.SeedWork;


public class UnitOfWork : IUnitOfWork
{
    private readonly BookingDbContext _bookingDbContext;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(BookingDbContext bookingDbContext)
    {
        _bookingDbContext = bookingDbContext;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (_repositories.TryGetValue(type, out var repo))
            return (IRepository<TEntity>)repo;

        var newRepo = new Repository<TEntity>(_bookingDbContext);
        _repositories[type] = newRepo;
        return newRepo;
    }

    public int SaveChanges() => _bookingDbContext.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _bookingDbContext.SaveChangesAsync(ct);
}