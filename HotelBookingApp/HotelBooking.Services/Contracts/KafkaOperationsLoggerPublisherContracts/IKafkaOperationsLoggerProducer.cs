namespace HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;

public interface IKafkaOperationsLoggerProducer
{
    public Task LogAsync(
        string entityType,
        string entityId,
        string operation,
        object? changes = null,
        string? actorId = null,
        string? actorType = null,
        string? source = null,
        CancellationToken ct = default);
}