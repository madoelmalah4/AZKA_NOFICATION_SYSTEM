using Azka.NotificationPlatform.Domain.Entities;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Repository contract for <see cref="NotificationHistory"/> persistence (FR-7).
/// History records are append-only; no update or delete operations are exposed.
/// </summary>
public interface INotificationHistoryRepository
{
    /// <summary>
    /// Returns the full chronological audit trail for a notification, ordered by
    /// <see cref="NotificationHistory.ChangedAt"/> ascending.
    /// </summary>
    Task<IReadOnlyList<NotificationHistory>> GetByNotificationIdAsync(
        Guid notificationId,
        CancellationToken cancellationToken = default);

    /// <summary>Appends a new immutable history entry to the store.</summary>
    Task AddAsync(NotificationHistory history, CancellationToken cancellationToken = default);
}
