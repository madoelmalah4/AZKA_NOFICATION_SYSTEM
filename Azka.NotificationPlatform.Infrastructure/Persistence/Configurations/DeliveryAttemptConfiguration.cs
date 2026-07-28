using Azka.NotificationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core Fluent API mapping for <see cref="DeliveryAttempt"/> (FR-7).
/// </summary>
internal sealed class DeliveryAttemptConfiguration : IEntityTypeConfiguration<DeliveryAttempt>
{
    public void Configure(EntityTypeBuilder<DeliveryAttempt> builder)
    {
        builder.ToTable("DeliveryAttempts");

        builder.HasKey(a => a.AttemptId);

        builder.Property(a => a.AttemptId)
               .ValueGeneratedNever();

        builder.Property(a => a.NotificationId)
               .IsRequired();

        builder.Property(a => a.AttemptNumber)
               .IsRequired();

        builder.Property(a => a.StartedAt)
               .IsRequired();

        builder.Property(a => a.CompletedAt)
               .IsRequired(false);

        builder.Property(a => a.Result)
               .HasMaxLength(50)
               .IsRequired(false);

        builder.Property(a => a.ProviderResponse)
               .HasColumnType("nvarchar(max)")
               .IsRequired(false);

        // FK relationship: a DeliveryAttempt belongs to one Notification
        builder.HasIndex(a => a.NotificationId)
               .HasDatabaseName("IX_DeliveryAttempts_NotificationId");

        // Composite unique: each (NotificationId, AttemptNumber) pair must be unique
        builder.HasIndex(a => new { a.NotificationId, a.AttemptNumber })
               .IsUnique()
               .HasDatabaseName("UX_DeliveryAttempts_NotificationId_AttemptNumber");
    }
}
