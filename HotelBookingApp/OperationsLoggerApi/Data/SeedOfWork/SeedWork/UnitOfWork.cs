using OperationsLoggerApi.Data.SeedOfWork.SeedWork.Repositories;
using OperationsLoggerApi.Data.SeedOfWork.SeedWork.RepositoriesInterfaces;

namespace OperationsLoggerApi.Data.SeedOfWork.SeedWork;


public class UnitOfWork : IUnitOfWork
{
    private readonly OpsLogDbContext _operationLogDbContext;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(OpsLogDbContext operationLogDbContext)
    {
        _operationLogDbContext = operationLogDbContext;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (_repositories.TryGetValue(type, out var repo))
            return (IRepository<TEntity>)repo;

        var newRepo = new Repository<TEntity>(_operationLogDbContext);
        _repositories[type] = newRepo;
        return newRepo;
    }

    public int SaveChanges() => _operationLogDbContext.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _operationLogDbContext.SaveChangesAsync(ct);
}