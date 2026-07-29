using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Infrastructure.Configuration;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Azka.NotificationPlatform.Infrastructure.Providers.Strategies;

public sealed class FirebasePushStrategy : INotificationProviderStrategy
{
    private readonly FirebaseSettings _settings;
    private readonly ILogger<FirebasePushStrategy> _logger;
    private static readonly object _lock = new();
    private static bool _isInitialized;

    public FirebasePushStrategy(IOptions<FirebaseSettings> settings, ILogger<FirebasePushStrategy> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<(bool IsSuccess, string ProviderResponse)> ExecuteAsync(
        Domain.Entities.Notification notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Preparing to send FCM push notification to {Token}", notification.Recipient);

        try
        {
            EnsureFirebaseInitialized();

            var message = new Message
            {
                Token = notification.Recipient,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = notification.Subject ?? "Notification",
                    Body = notification.Body
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
            _logger.LogInformation("FCM Push notification sent successfully. MessageId: {MessageId}", response);

            return (true, $"{{\"status\":\"success\",\"messageId\":\"{response}\",\"provider\":\"Firebase\"}}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FCM Push notification dispatch failed.");
            return (false, $"{{\"status\":\"failed\",\"error\":\"{ex.Message}\",\"provider\":\"Firebase\"}}");
        }
    }

    private void EnsureFirebaseInitialized()
    {
        if (_isInitialized) return;

        lock (_lock)
        {
            if (_isInitialized) return;

            if (FirebaseApp.DefaultInstance == null)
            {
                _logger.LogInformation("Initializing FirebaseAdmin SDK with provided service account credential JSON.");
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(_settings.CredentialJson)
                });
            }

            _isInitialized = true;
        }
    }
}
