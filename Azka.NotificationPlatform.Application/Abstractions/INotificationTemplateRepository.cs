using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Repository contract for <see cref="NotificationTemplate"/> persistence (FR-3).
/// </summary>
public interface INotificationTemplateRepository
{
    /// <summary>Retrieves a template by its surrogate key.</summary>
    Task<NotificationTemplate?> GetByIdAsync(Guid templateId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves the highest active version of a template matching the given
    /// notification type, channel, and language tag.
    /// Returns <see langword="null"/> if no active template is found.
    /// </summary>
    Task<NotificationTemplate?> GetActiveTemplateAsync(
        string notificationType,
        NotificationChannel channel,
        string language,
        CancellationToken cancellationToken = default);

    /// <summary>Returns all templates, optionally filtered by channel.</summary>
    Task<IReadOnlyList<NotificationTemplate>> GetAllAsync(
        NotificationChannel? channel = null,
        CancellationToken cancellationToken = default);

    /// <summary>Persists a new <see cref="NotificationTemplate"/>.</summary>
    Task AddAsync(NotificationTemplate template, CancellationToken cancellationToken = default);

    /// <summary>Marks a template as modified in the unit of work.</summary>
    void Update(NotificationTemplate template);
}
