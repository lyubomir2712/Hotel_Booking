using OperationsLoggerApi.Infrastructure.AutoMapper.DTOs;

namespace OperationsLoggerApi.Interfaces;

public interface IAddOperationLogToRedis
{
    public Task AddOperationLogToRedisAsync(OpsLogEntryDto entry, CancellationToken ct = default);
}