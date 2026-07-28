using Azka.NotificationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core Fluent API mapping for <see cref="NotificationTemplate"/> (FR-3).
/// </summary>
internal sealed class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.ToTable("NotificationTemplates");

        builder.HasKey(t => t.TemplateId);

        builder.Property(t => t.TemplateId)
               .ValueGeneratedNever();

        builder.Property(t => t.TemplateName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Channel)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(t => t.Subject)
               .HasMaxLength(998);

        builder.Property(t => t.Body)
               .IsRequired()
               .HasColumnType("nvarchar(max)");

        builder.Property(t => t.Language)
               .IsRequired()
               .HasMaxLength(10); // BCP-47 tags are short

        builder.Property(t => t.Version)
               .IsRequired();

        builder.Property(t => t.Status)
               .IsRequired()
               .HasMaxLength(20); // "Active", "Inactive", "Archived"

        // Composite index supports the active-template resolution query
        builder.HasIndex(t => new { t.Channel, t.Language, t.Status, t.Version })
               .HasDatabaseName("IX_NotificationTemplates_Channel_Language_Status_Version");
    }
}
