using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using OperationsLoggerApi.Infrastructure.AutoMapper.DTOs;
using OperationsLoggerApi.Interfaces;
using Microsoft.Extensions.Logging;

namespace OperationsLoggerApi.Infrastructure;

public class AddOperationLogToRedis : IAddOperationLogToRedis
{
    private readonly IDatabase _db;
    private readonly ILogger<AddOperationLogToRedis> _logger;

    public AddOperationLogToRedis(IConnectionMultiplexer multiplexer, ILogger<AddOperationLogToRedis> logger)
    {
        if (multiplexer == null) throw new ArgumentNullException(nameof(multiplexer));
        _db = multiplexer.GetDatabase();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddOperationLogToRedisAsync(OpsLogEntryDto entry, CancellationToken ct = default)
    {
        if (entry == null) throw new ArgumentNullException(nameof(entry));
        
        string keyId = !string.IsNullOrWhiteSpace(entry.EventId) 
            ? entry.EventId 
            : (entry.Id != 0 ? entry.Id.ToString() : Guid.NewGuid().ToString());

        string key = $"Operation_{keyId}";

        string json = JsonSerializer.Serialize(entry);

        try
        {
            bool result = await _db.StringSetAsync(key, json, TimeSpan.FromDays(30)).ConfigureAwait(false);
            if (result)
            {
                _logger.LogInformation("Successfully stored operation log with key {Key}", key);
            }
            else
            {
                _logger.LogWarning("Failed to store operation log with key {Key}", key);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while storing operation log with key {Key}", key);
        }
    }
}