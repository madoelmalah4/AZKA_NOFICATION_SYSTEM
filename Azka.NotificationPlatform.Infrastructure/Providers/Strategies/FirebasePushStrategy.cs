using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;

namespace Azka.NotificationPlatform.Infrastructure.Providers.Strategies;

public sealed class FirebasePushStrategy : INotificationProviderStrategy
{
    public Task<(bool IsSuccess, string ProviderResponse)> ExecuteAsync(Notification notification, CancellationToken cancellationToken)
    {
        // Mocking push notification gateway delivery
        bool isSuccess = true;
        string response = $"{{\"messageId\":\"fcm-{Guid.NewGuid():N}\",\"status\":\"success\",\"provider\":\"Firebase\"}}";
        return Task.FromResult((isSuccess, response));
    }
}
