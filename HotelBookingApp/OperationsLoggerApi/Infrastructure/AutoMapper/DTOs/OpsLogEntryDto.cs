namespace OperationsLoggerApi.Infrastructure.AutoMapper.DTOs;


public class OpsLogEntryDto
{
    public int Id { get; set; }
    public string? EventId { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
    public string? TenantId { get; set; }
    public string? ActorId { get; set; }
    public string? ActorType { get; set; }
    public string? Source { get; set; }
    public string? EntityType { get; set; }
    public string? EntityId { get; set; }
    public string? Operation { get; set; }
    public string Changes { get; set; }

    public OpsLogEntryDto() { }

    public OpsLogEntryDto(int id, string? eventId, DateTimeOffset occurredAt, string? tenantId,
                          string? actorId, string? actorType, string? source, string? entityType,
                          string? entityId, string? operation, string changes)
    {
        Id = id;
        EventId = eventId;
        OccurredAt = occurredAt;
        TenantId = tenantId;
        ActorId = actorId;
        ActorType = actorType;
        Source = source;
        EntityType = entityType;
        EntityId = entityId;
        Operation = operation;
        Changes = changes;
    }
}
