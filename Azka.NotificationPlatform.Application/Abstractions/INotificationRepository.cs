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
}
