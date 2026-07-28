namespace Azka.NotificationPlatform.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of a <see cref="Entities.Notification"/> as it
/// travels through the notification pipeline.
/// </summary>
/// <remarks>
/// State transitions (happy path):
///   Pending → Queued → Processing → Delivered
/// Failure/cancellation paths:
///   Processing → Failed
///   Pending | Queued → Cancelled
/// </remarks>
public enum NotificationStatus
{
    /// <summary>
    /// The notification has been accepted by the platform but has not yet been
    /// placed on the processing queue. This is the initial state after intake.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// The notification has been enqueued and is awaiting a worker to pick it up.
    /// </summary>
    Queued = 1,

    /// <summary>
    /// A worker has dequeued the notification and is actively attempting delivery
    /// via the selected <see cref="NotificationChannel"/>.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// The downstream provider has confirmed successful delivery to the recipient.
    /// This is a terminal success state.
    /// </summary>
    Delivered = 3,

    /// <summary>
    /// All delivery attempts have been exhausted without a successful acknowledgement.
    /// This is a terminal failure state.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// The notification was explicitly cancelled before delivery was completed,
    /// either by the originating system or by an administrator.
    /// This is a terminal state.
    /// </summary>
    Cancelled = 5
}
