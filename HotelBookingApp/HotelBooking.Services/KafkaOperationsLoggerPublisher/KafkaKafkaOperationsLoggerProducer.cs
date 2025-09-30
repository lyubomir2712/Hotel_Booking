using System.Text.Json;
using Confluent.Kafka;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;

namespace HotelBooking.Services.KafkaOperationsLoggerPublisher;

public sealed class KafkaKafkaOperationsLoggerProducer : IKafkaOperationsLoggerProducer, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public KafkaKafkaOperationsLoggerProducer(KafkaOptions options)
    {
        _topic = options.Topic;
        var cfg = new ProducerConfig {
            BootstrapServers = options.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            LingerMs = options.LingerMs
        };
        _producer = new ProducerBuilder<string,string>(cfg).Build();
    }

    public async Task LogAsync(
        string entityType,
        string entityId,
        string operation,
        object? changes = null,
        string? actorId = null,
        string? actorType = null,
        string? source = null,
        CancellationToken ct = default)
    {
        var evt = new {
            EventId = Guid.NewGuid().ToString("N"),
            OccurredAt = DateTimeOffset.UtcNow,
            ActorId = actorId ?? "system",
            ActorType = actorType ?? "System",
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