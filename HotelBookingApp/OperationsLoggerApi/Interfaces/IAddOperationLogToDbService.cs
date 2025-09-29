using AutoMapper;
using OperationsLoggerApi.Data.Models;
using OperationsLoggerApi.Infrastructure;
using OperationsLoggerApi.Infrastructure.AutoMapper.DTOs;

namespace OperationsLoggerApi.Interfaces;

public interface IAddOperationLogToDbService
{
    public Task<int> AddOperationLogToDbAsync(OpsLogEntryDto entry, CancellationToken ct = default);
}