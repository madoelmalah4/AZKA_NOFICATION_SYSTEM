using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Factory pattern contract to dynamically resolve notification provider strategies (FR-12).
/// </summary>
public interface IProviderFactory
{
    /// <summary>
    /// Resolves the strategy instance bound to the target notification channel.
    /// </summary>
    /// <param name="channel">The notification delivery channel.</param>
    /// <returns>The resolved INotificationProviderStrategy instance.</returns>
    INotificationProviderStrategy GetStrategy(NotificationChannel channel);
}
