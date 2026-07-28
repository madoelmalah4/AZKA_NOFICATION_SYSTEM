using Azka.NotificationPlatform.Domain.Entities;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Strategy pattern contract for executing notification delivery against a concrete provider (FR-12).
/// </summary>
public interface INotificationProviderStrategy
{
    /// <summary>
    /// Executes the dispatch using the specific vendor client or HTTP payload.
    /// </summary>
    /// <param name="notification">The notification to dispatch.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A tuple indicating success and the raw provider response string.</returns>
    Task<(bool IsSuccess, string ProviderResponse)> ExecuteAsync(Notification notification, CancellationToken cancellationToken);
}
