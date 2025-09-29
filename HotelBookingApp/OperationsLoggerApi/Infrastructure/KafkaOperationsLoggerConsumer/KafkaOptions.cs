namespace OperationsLoggerApi.KafkaOperationsLoggerConsumer;

public sealed class KafkaOptions
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string Topic { get; set; } = "ops-log";
    public string GroupId { get; set; } = "ops-writer";
    public string AutoOffsetReset { get; set; } = "Earliest"; 
}