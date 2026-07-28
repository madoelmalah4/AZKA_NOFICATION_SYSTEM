using System.Threading.Channels;
using Azka.NotificationPlatform.Application.Abstractions;

namespace Azka.NotificationPlatform.Infrastructure.Queue;

/// <summary>
/// Thread-safe in-memory queue using System.Threading.Channels (FR-4).
/// Bounded to 50,000 items to protect memory boundaries under extreme workloads.
/// </summary>
public sealed class InMemoryNotificationQueue : INotificationQueue
{
    private readonly Channel<Guid> _channel;

    public InMemoryNotificationQueue()
    {
        var options = new BoundedChannelOptions(50000)
        {
            FullMode = BoundedChannelFullMode.Wait, // Block/wait when full to apply backpressure
            SingleReader = true,                   // Background worker runs sequentially (single thread worker loop)
            SingleWriter = false                    // Multiple API requests can write concurrently
        };
        _channel = Channel.CreateBounded<Guid>(options);
    }

    /// <inheritdoc />
    public async ValueTask EnqueueAsync(Guid notificationId)
    {
        await _channel.Writer.WriteAsync(notificationId);
    }

    /// <inheritdoc />
    public async ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }
}
