using Azka.NotificationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Azka.NotificationPlatform.Infrastructure.Persistence;

/// <summary>
/// EF Core <see cref="DbContext"/> for the Azka Notification Platform.
/// Owns all five aggregate tables. Configurations are applied via separate
/// <see cref="IEntityTypeConfiguration{TEntity}"/> classes discovered by
/// <see cref="ModelBuilder.ApplyConfigurationsFromAssembly"/>.
/// </summary>
public sealed class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options) { }

    /// <summary>All notification requests persisted by the platform.</summary>
    public DbSet<Notification> Notifications { get; set; } = null!;

    /// <summary>All versioned message templates (FR-3).</summary>
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; } = null!;

    /// <summary>All registered downstream provider records.</summary>
    public DbSet<NotificationProvider> NotificationProviders { get; set; } = null!;

    /// <summary>All delivery attempt records for all notifications (FR-7).</summary>
    public DbSet<DeliveryAttempt> DeliveryAttempts { get; set; } = null!;

    /// <summary>The complete append-only status-transition ledger (FR-7).</summary>
    public DbSet<NotificationHistory> NotificationHistories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Discover and apply all IEntityTypeConfiguration<T> classes in this assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
