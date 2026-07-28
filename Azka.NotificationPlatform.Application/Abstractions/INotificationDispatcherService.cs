using Azka.NotificationPlatform.Domain.Entities;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Service contract for dispatching a <see cref="Notification"/> through the
/// appropriate downstream provider channel.
/// </summary>
/// <remarks>
/// Concrete implementations live in the Infrastructure Layer, one per
/// <see cref="Domain.Enums.NotificationChannel"/>. The Application Layer resolves
/// the correct implementation at runtime using the channel value on the notification.
/// This interface deliberately carries no channel knowledge — selection is the
/// responsibility of the caller (typically a MediatR command handler).
/// </remarks>
public interface INotificationDispatcherService
{
    /// <summary>
    /// Dispatches the notification through the provider, records a
    /// <see cref="DeliveryAttempt"/>, and returns the result string
    /// (e.g., <c>"Success"</c>, <c>"Failure"</c>).
    /// </summary>
    /// <param name="notification">The notification to dispatch.</param>
    /// <param name="provider">The selected downstream provider.</param>
    /// <param name="cancellationToken">Propagates cancellation from the caller.</param>
    /// <returns>A tuple of (result label, raw provider response).</returns>
    Task<(string Result, string? ProviderResponse)> DispatchAsync(
        Notification notification,
        NotificationProvider provider,
        CancellationToken cancellationToken = default);
}
