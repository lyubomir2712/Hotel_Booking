using OperationsLoggerApi.Data.Models;

namespace OperationsLoggerApi.Interfaces;

public interface IAddOperationLogToDbService
{
    public Task<int> AddOperationLogToDbAsync(OpsLogEntryModel entry, CancellationToken ct = default);
}