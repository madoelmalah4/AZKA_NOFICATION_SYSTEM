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
    /// <returns>A tuple indicating success, provider response, and whether a failure is recoverable for retry logic.</returns>
    Task<(bool IsSuccess, string ProviderResponse, bool IsRecoverable)> ExecuteAsync(Notification notification, CancellationToken cancellationToken);
}
