namespace HotelBooking.Services.KafkaOperationsLoggerPublisher;

public class KafkaOptions
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string Topic { get; set; } = "ops-log";
    public int LingerMs { get; set; } = 5;
}