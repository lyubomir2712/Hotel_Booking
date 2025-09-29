using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OperationsLoggerApi.Interfaces;
using OperationsLoggerApi.KafkaOperationsLoggerConsumer;
using Microsoft.Extensions.DependencyInjection;
using OperationsLoggerApi.Infrastructure.AutoMapper.DTOs;

namespace OperationsLoggerApi.Infrastructure.KafkaOperationsLoggerConsumer;

public sealed class OpsLogConsumer : BackgroundService
{
    private readonly ILogger<OpsLogConsumer> _log;
    private readonly KafkaOptions _opt;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IServiceScopeFactory _scopeFactory;
    
    public OpsLogConsumer(ILogger<OpsLogConsumer> log, IOptions<KafkaOptions> opt,
        IHostApplicationLifetime lifetime, IServiceScopeFactory scopeFactory)
    {
        _log = log;
        _opt = opt.Value;
        _lifetime = lifetime;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_lifetime.ApplicationStarted.IsCancellationRequested)
        {
            _log.LogInformation("OpsLogConsumer waiting for ApplicationStarted...");
            var tcs = new TaskCompletionSource();
            using var reg1 = _lifetime.ApplicationStarted.Register(() => tcs.TrySetResult());
            using var reg2 = stoppingToken.Register(() => tcs.TrySetCanceled(stoppingToken));
            await tcs.Task;
        }

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

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var cr = consumer.Consume(TimeSpan.FromMilliseconds(250));
                if (cr is null) continue;

                try
                {
                    using var doc = JsonDocument.Parse(cr.Message.Value);
                    
                    var dto = new OpsLogEntryDto
                    {
                        EventId = doc.RootElement.GetProperty("EventId").GetString(),
                        OccurredAt = doc.RootElement.GetProperty("OccurredAt").GetDateTimeOffset(),
                        TenantId = doc.RootElement.GetProperty("TenantId").GetString(),
                        ActorId = doc.RootElement.GetProperty("ActorId").GetString(),
                        ActorType = doc.RootElement.GetProperty("ActorType").GetString(),
                        Source = doc.RootElement.GetProperty("Source").GetString(),
                        EntityType = doc.RootElement.GetProperty("EntityType").GetString(),
                        EntityId = doc.RootElement.GetProperty("EntityId").GetString(),
                        Operation = doc.RootElement.GetProperty("Operation").GetString(),
                        Changes = doc.RootElement.GetProperty("Changes").GetRawText()
                    };
                    
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var addSvc = scope.ServiceProvider.GetRequiredService<IAddOperationLogToDbService>();
                        await addSvc.AddOperationLogToDbAsync(dto, stoppingToken);
                    }

                    _log.LogInformation("Consumed {TPO} | {Key} -> {EntityType}/{EntityId} {Operation}",
                        cr.TopicPartitionOffset, cr.Message.Key, dto.EntityType, dto.EntityId, dto.Operation);

                    consumer.Commit(cr);
                }
                catch (Exception exProcess)
                {
                    _log.LogError(exProcess, "Failed processing message at {TPO}. Skipping (no commit).",
                        cr.TopicPartitionOffset);
                }
            }
        }
        catch (OperationCanceledException ex)
        {
            _log.LogInformation(ex, "Kafka consumer stopped due to cancellation.");
        }
        finally
        {
            consumer.Close(); 
            _log.LogInformation("OpsLogConsumer stopped.");
        }

        await Task.CompletedTask;
    }
}