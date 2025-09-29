using OperationsLoggerApi.Data.Models;

namespace OperationsLoggerApi.Data;
using Microsoft.EntityFrameworkCore;


public class OpsLogDbContext : DbContext
{
    public OpsLogDbContext(DbContextOptions<OpsLogDbContext> options)
        : base(options)
    {
    }

    public DbSet<OpsLogEntryModel> OpsLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OpsLogEntryModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EventId).IsRequired().HasMaxLength(64);
            entity.Property(e => e.OccurredAt).IsRequired();
            entity.Property(e => e.TenantId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ActorId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ActorType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Source).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EntityType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EntityId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Operation).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Changes).IsRequired();
        });
    }
}