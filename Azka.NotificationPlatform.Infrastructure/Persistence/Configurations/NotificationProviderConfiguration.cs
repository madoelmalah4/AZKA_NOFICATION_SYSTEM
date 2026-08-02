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

        builder.HasData(
            new NotificationProvider(
                Guid.Parse("b1111111-1111-1111-1111-111111111111"),
                "SendGrid",
                Domain.Enums.NotificationChannel.Email,
                isActive: true
            ),
            new NotificationProvider(
                Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                "Twilio",
                Domain.Enums.NotificationChannel.SMS,
                isActive: true
            ),
            new NotificationProvider(
                Guid.Parse("b3333333-3333-3333-3333-333333333333"),
                "Firebase",
                Domain.Enums.NotificationChannel.Push,
                isActive: true
            )
        );
    }
}
