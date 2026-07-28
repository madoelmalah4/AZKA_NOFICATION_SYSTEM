namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Non-blocking high-throughput queue contract using System.Threading.Channels (FR-4).
/// </summary>
public interface INotificationQueue
{
    /// <summary>
    /// Enqueues a notification ID to be processed asynchronously.
    /// </summary>
    ValueTask EnqueueAsync(Guid notificationId);

    /// <summary>
    /// Dequeues a notification ID for processing. Blocks until an item is available.
    /// </summary>
    ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken);
}
