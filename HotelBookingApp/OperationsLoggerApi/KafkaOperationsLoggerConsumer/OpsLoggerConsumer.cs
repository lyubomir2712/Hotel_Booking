using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace OperationsLoggerApi.KafkaOperationsLoggerConsumer;

public sealed class OpsLogConsumer : BackgroundService
{
    private readonly ILogger<OpsLogConsumer> _log;
    private readonly KafkaOptions _opt;
    private readonly IHostApplicationLifetime _lifetime;

    public OpsLogConsumer(ILogger<OpsLogConsumer> log, IOptions<KafkaOptions> opt, IHostApplicationLifetime lifetime)
        => (_log, _opt, _lifetime) = (log, opt.Value, lifetime);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var conf = new ConsumerConfig
        {
            BootstrapServers = _opt.BootstrapServers,
            GroupId = _opt.GroupId,
            AutoOffsetReset = Enum.TryParse<AutoOffsetReset>(_opt.AutoOffsetReset, true, out var aor) ? aor : AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(conf).Build();
        consumer.Subscribe(_opt.Topic);

        _log.LogInformation("OpsLogConsumer started. Topic={Topic}, Group={Group}", _opt.Topic, _opt.GroupId);

        // Ensure the web host finishes binding before we enter the consume loop
        if (!_lifetime.ApplicationStarted.IsCancellationRequested)
        {
            var tcs = new TaskCompletionSource();
            using var reg1 = _lifetime.ApplicationStarted.Register(() => tcs.TrySetResult());
            using var reg2 = stoppingToken.Register(() => tcs.TrySetCanceled(stoppingToken));
            await tcs.Task;
        }

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Timed poll so the loop stays responsive and the program continues while listening
                var cr = consumer.Consume(TimeSpan.FromMilliseconds(250));
                if (cr is null) continue;

                try
                {
                    // Parse payload (adjust to your contract if needed)
                    using var doc = JsonDocument.Parse(cr.Message.Value);
                    var entityType = doc.RootElement.GetProperty("EntityType").GetString();
                    var entityId   = doc.RootElement.GetProperty("EntityId").GetString();
                    var operation  = doc.RootElement.GetProperty("Operation").GetString();

                    // TODO: persist to DB or call your handler
                    _log.LogInformation("Consumed {TPO} | {Key} -> {EntityType}/{EntityId} {Operation}",
                        cr.TopicPartitionOffset, cr.Message.Key, entityType, entityId, operation);

                    consumer.Commit(cr); // commit after successful processing
                }
                catch (Exception exProcess)
                {
                    _log.LogError(exProcess, "Failed processing message at {TPO}. Skipping (no commit).", cr.TopicPartitionOffset);
                    // Optionally: DLQ logic here
                }
            }
        }
        catch (OperationCanceledException) { /* graceful shutdown */ }
        finally
        {
            consumer.Close(); // commits last processed offsets on close if needed
            _log.LogInformation("OpsLogConsumer stopped.");
        }

        await Task.CompletedTask;
    }
}