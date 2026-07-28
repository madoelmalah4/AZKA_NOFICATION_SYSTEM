using Azka.NotificationPlatform.Domain.Entities;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Repository contract for <see cref="DeliveryAttempt"/> persistence (FR-7).
/// </summary>
public interface IDeliveryAttemptRepository
{
    /// <summary>Retrieves a single delivery attempt by its surrogate key.</summary>
    Task<DeliveryAttempt?> GetByIdAsync(Guid attemptId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all delivery attempts for a given notification, ordered by
    /// <see cref="DeliveryAttempt.AttemptNumber"/> ascending.
    /// </summary>
    Task<IReadOnlyList<DeliveryAttempt>> GetByNotificationIdAsync(
        Guid notificationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the highest <see cref="DeliveryAttempt.AttemptNumber"/> recorded for
    /// a notification, or 0 if no attempts exist yet. Used to compute the next ordinal.
    /// </summary>
    Task<int> GetMaxAttemptNumberAsync(Guid notificationId, CancellationToken cancellationToken = default);

    /// <summary>Persists a new <see cref="DeliveryAttempt"/>.</summary>
    Task AddAsync(DeliveryAttempt attempt, CancellationToken cancellationToken = default);

    /// <summary>Marks an attempt as modified in the unit of work.</summary>
    void Update(DeliveryAttempt attempt);
}
