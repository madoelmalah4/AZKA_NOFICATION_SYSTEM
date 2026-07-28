using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Infrastructure.Providers.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace Azka.NotificationPlatform.Infrastructure.Providers;

/// <summary>
/// Service Locator/Factory wrapping IServiceProvider to dynamically pull strategies (FR-12).
/// </summary>
public sealed class ProviderFactory : IProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public INotificationProviderStrategy GetStrategy(NotificationChannel channel)
    {
        return channel switch
        {
            NotificationChannel.Email => _serviceProvider.GetRequiredService<SendGridEmailStrategy>(),
            NotificationChannel.SMS   => _serviceProvider.GetRequiredService<TwilioSmsStrategy>(),
            NotificationChannel.Push  => _serviceProvider.GetRequiredService<FirebasePushStrategy>(),
            _ => throw new ArgumentException($"Channel {channel} has no matching delivery strategy.", nameof(channel))
        };
    }
}
