using OperationsLoggerApi.Data.SeedOfWork.SeedWork.RepositoriesInterfaces;

namespace OperationsLoggerApi.Data.SeedOfWork.SeedWork;
public interface IUnitOfWork
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}



