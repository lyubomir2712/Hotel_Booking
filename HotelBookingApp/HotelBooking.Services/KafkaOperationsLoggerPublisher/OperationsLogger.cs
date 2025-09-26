using System.Text.Json;
using Confluent.Kafka;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;

namespace HotelBooking.Services.KafkaOperationsLoggerPublisher;

public sealed class KafkaOperationsLogger : IOperationsLogger, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public KafkaOperationsLogger(string bootstrapServers="localhost:9092", string topic="ops-log")
    {
        _topic = topic;
        var cfg = new ProducerConfig {
            BootstrapServers = bootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            LingerMs = 5
        };
        _producer = new ProducerBuilder<string,string>(cfg).Build();
    }

    public async Task LogAsync(string entityType, string entityId, string operation, object? changes = null,
        string? tenantId = null, string? actorId = null, string? source = null,
        CancellationToken ct = default)
    {
        var evt = new {
            EventId = Guid.NewGuid().ToString("N"),
            OccurredAt = DateTimeOffset.UtcNow,
            TenantId = tenantId ?? "default",
            ActorId = actorId ?? "system",
            ActorType = "System",
            Source = source ?? "App",
            EntityType = entityType,
            EntityId = entityId,
            Operation = operation,
            Changes = changes
        };
        var key = $"{entityType}:{entityId}";
        await _producer.ProduceAsync(_topic, new Message<string,string>{
            Key = key, Value = JsonSerializer.Serialize(evt)
        }, ct);
    }

    public void Dispose() => _producer.Flush(TimeSpan.FromSeconds(5));
}