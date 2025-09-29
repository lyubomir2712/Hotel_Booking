using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OperationsLoggerApi.Data;
using OperationsLoggerApi.Data.Models;

namespace OperationsLoggerApi.KafkaOperationsLoggerConsumer
{
    public class AddOperationLogToDbService
    {
        private readonly OpsLogDbContext _db;
        private readonly ILogger<AddOperationLogToDbService> _logger;

        public AddOperationLogToDbService(OpsLogDbContext db, ILogger<AddOperationLogToDbService> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> AddOperationLogToDbAsync(OpsLogEntryModel entry, CancellationToken ct = default)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            entry.OccurredAt = entry.OccurredAt.ToUniversalTime();

            _db.OpsLogs.Add(entry);

            try
            {
                await _db.SaveChangesAsync(ct);
                _logger.LogInformation(
                    "Saved OpsLogEntry {EventId} for {EntityType}:{EntityId}",
                    entry.EventId,
                    entry.EntityType,
                    entry.EntityId);
                return entry.Id;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save OpsLogEntry {EventId}", entry.EventId);
                throw;
            }
        }
    }
}