using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Repository contract for <see cref="Notification"/> aggregate persistence.
/// Implementations live in the Infrastructure Layer; this interface keeps the
/// Application Layer decoupled from any specific ORM or database technology.
/// </summary>
public interface INotificationRepository
{
    /// <summary>Retrieves a notification by its platform-assigned surrogate key.</summary>
    Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a notification by the caller-supplied idempotency key (FR-11).
    /// Returns <see langword="null"/> if no notification with that key exists.
    /// </summary>
    Task<Notification?> GetByCorrelationIdAsync(Guid correlationId, CancellationToken cancellationToken = default);

    /// <summary>Returns all notifications for a specific recipient address.</summary>
    Task<IReadOnlyList<Notification>> GetByRecipientAsync(string recipient, CancellationToken cancellationToken = default);

    /// <summary>Returns all notifications currently in the given status.</summary>
    Task<IReadOnlyList<Notification>> GetByStatusAsync(NotificationStatus status, CancellationToken cancellationToken = default);

    /// <summary>Persists a new <see cref="Notification"/> to the store.</summary>
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);

    /// <summary>Marks an existing <see cref="Notification"/> as modified in the unit of work.</summary>
    void Update(Notification notification);

    /// <summary>Deletes an existing <see cref="Notification"/> from the store.</summary>
    void Remove(Notification notification);

    /// <summary>
    /// Returns a dictionary of Status (int) → Count for all notifications.
    /// Used by the dashboard summary endpoint (FR-10).
    /// </summary>
    Task<Dictionary<int, int>> GetStatusCountsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches and filters notifications with database-level pagination (FR-9).
    /// </summary>
    Task<DTOs.PagedResult<DTOs.NotificationDto>> SearchAsync(
        Features.Notifications.Queries.SearchNotificationsQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns notification counts grouped by Channel and Status.
    /// Key: (Channel int, Status int), Value: Count.
    /// Used by the per-channel dashboard endpoint (FR-10).
    /// </summary>
    Task<Dictionary<(int Channel, int Status), int>> GetCountByChannelAndStatusAsync(CancellationToken cancellationToken = default);
}
