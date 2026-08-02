using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;

namespace Azka.NotificationPlatform.Infrastructure.Providers.Strategies;

public sealed class TwilioSmsStrategy : INotificationProviderStrategy
{
    public Task<(bool IsSuccess, string ProviderResponse, bool IsRecoverable)> ExecuteAsync(Notification notification, CancellationToken cancellationToken)
    {
        // Mocking SMS gateway delivery
        bool isSuccess = true;
        string response = $"{{\"sid\":\"SM{Guid.NewGuid():N}\",\"status\":\"sent\",\"provider\":\"Twilio\"}}";
        return Task.FromResult((isSuccess, response, true));
    }
}
