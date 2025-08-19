using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace HotelBooking.Data.SeedWork;
public interface IUnitOfWork
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}



