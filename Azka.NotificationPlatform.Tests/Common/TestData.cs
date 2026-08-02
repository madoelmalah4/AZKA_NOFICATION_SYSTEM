using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Tests.Common;

public static class TestData
{
    public static Notification CreateNotification(
        Guid? id = null,
        string type = "UserRegistration",
        string recipient = "user@example.com",
        NotificationChannel channel = NotificationChannel.Email,
        string body = "Test Body",
        Guid? correlationId = null,
        DateTime? requestedAt = null,
        string? subject = "Test Subject",
        string? appName = "TestApp")
    {
        return new Notification(
            id ?? Guid.NewGuid(),
            type,
            recipient,
            channel,
            body,
            correlationId ?? Guid.NewGuid(),
            requestedAt ?? DateTime.UtcNow,
            subject,
            appName);
    }
}
