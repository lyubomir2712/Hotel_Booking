namespace HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;

public interface IOperationsLogger
{
    Task LogAsync(string entityType, string entityId, string operation, object? changes = null,
        string? tenantId = null, string? actorId = null, string? source = null,
        CancellationToken ct = default);
}