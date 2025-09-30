using Microsoft.EntityFrameworkCore;

namespace OperationsLoggerApi.Data.Models;

public class OpsLogEntryModel : BaseModel
{
    
    public string EventId { get; set; } = string.Empty; // Unique ID of this specific event (GUID) 
    public DateTimeOffset OccurredAt { get; set; } // Timestamp of when the event actually happened (UTC) 
    public string ActorId { get; set; } = string.Empty; // Who triggered the action (user/service/system) 
    public string ActorType { get; set; } = string.Empty; // Type of actor (e.g. User, Admin, System) 
    public string Source { get; set; } = string.Empty; // Where the event originated from (subsystem or app) 
    public string EntityType { get; set; } = string.Empty; // Type of entity affected (e.g. Booking, Customer) 
    public string EntityId { get; set; } = string.Empty; // ID of the specific entity affected 
    public string Operation { get; set; } = string.Empty; // What operation occurred (e.g. CREATE, UPDATE) 
    public string Changes { get; set; } = string.Empty; // Data changes (often stored as JSON diff)
}