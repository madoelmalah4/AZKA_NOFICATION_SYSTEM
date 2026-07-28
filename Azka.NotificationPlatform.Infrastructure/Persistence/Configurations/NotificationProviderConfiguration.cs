using Azka.NotificationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core Fluent API mapping for <see cref="NotificationProvider"/>.
/// </summary>
internal sealed class NotificationProviderConfiguration : IEntityTypeConfiguration<NotificationProvider>
{
    public void Configure(EntityTypeBuilder<NotificationProvider> builder)
    {
        builder.ToTable("NotificationProviders");

        builder.HasKey(p => p.ProviderId);

        builder.Property(p => p.ProviderId)
               .ValueGeneratedNever();

        builder.Property(p => p.ProviderName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(p => p.Channel)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(p => p.IsActive)
               .IsRequired();

        // Unique provider name prevents duplicate registrations
        builder.HasIndex(p => p.ProviderName)
               .IsUnique()
               .HasDatabaseName("UX_NotificationProviders_ProviderName");

        // Index for the active-by-channel provider selection query
        builder.HasIndex(p => new { p.Channel, p.IsActive })
               .HasDatabaseName("IX_NotificationProviders_Channel_IsActive");
    }
}
