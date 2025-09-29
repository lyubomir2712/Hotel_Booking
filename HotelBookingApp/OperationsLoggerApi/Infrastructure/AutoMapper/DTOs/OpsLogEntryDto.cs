namespace OperationsLoggerApi.Infrastructure.AutoMapper.DTOs;

public record OpsLogEntryDto(
    int Id,
    string EventId,
    DateTimeOffset OccurredAt,
    string TenantId,
    string ActorId,
    string ActorType,
    string Source,
    string EntityType,
    string EntityId,
    string Operation,
    string Changes
);