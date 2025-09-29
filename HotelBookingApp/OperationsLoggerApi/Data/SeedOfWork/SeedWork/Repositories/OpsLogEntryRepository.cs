using OperationsLoggerApi.Data.Models;
using OperationsLoggerApi.Data.SeedOfWork.SeedWork.RepositoriesInterfaces;

namespace OperationsLoggerApi.Data.SeedOfWork.SeedWork.Repositories;

public class OpsLogEntryRepository(OpsLogDbContext opsLogDbContext) : Repository<OpsLogEntryModel>(opsLogDbContext), IOpsLogEntryRepository;