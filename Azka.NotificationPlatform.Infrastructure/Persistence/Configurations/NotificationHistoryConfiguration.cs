using Azka.NotificationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core Fluent API mapping for <see cref="NotificationHistory"/> (FR-7).
/// The table is configured as purely append-only — no update or delete
/// operations should be issued against it.
/// </summary>
internal sealed class NotificationHistoryConfiguration : IEntityTypeConfiguration<NotificationHistory>
{
    public void Configure(EntityTypeBuilder<NotificationHistory> builder)
    {
        builder.ToTable("NotificationHistories");

        builder.HasKey(h => h.HistoryId);

        builder.Property(h => h.HistoryId)
               .ValueGeneratedNever();

        builder.Property(h => h.NotificationId)
               .IsRequired();

        builder.Property(h => h.Status)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(h => h.ChangedAt)
               .IsRequired();

        builder.Property(h => h.Remarks)
               .HasMaxLength(1000)
               .IsRequired(false);

        // Primary query pattern: retrieve all history for a notification, by time
        builder.HasIndex(h => new { h.NotificationId, h.ChangedAt })
               .HasDatabaseName("IX_NotificationHistories_NotificationId_ChangedAt");
    }
}
