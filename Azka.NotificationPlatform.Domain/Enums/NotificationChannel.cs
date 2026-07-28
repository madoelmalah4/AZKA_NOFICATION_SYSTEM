namespace Azka.NotificationPlatform.Domain.Enums;

/// <summary>
/// Identifies the delivery channel through which a notification is dispatched
/// to the intended recipient.
/// </summary>
/// <remarks>
/// Each channel maps to one or more <see cref="Entities.NotificationProvider"/> records
/// that are capable of handling that channel type. The channel value is stored on the
/// <see cref="Entities.Notification"/> aggregate and on the provider record to enable
/// deterministic provider selection at dispatch time.
/// </remarks>
public enum NotificationChannel
{
    /// <summary>
    /// Electronic mail delivered via an SMTP relay or a transactional email API
    /// (e.g., SendGrid, Amazon SES, Postmark).
    /// </summary>
    Email = 0,

    /// <summary>
    /// Short Message Service (SMS) delivered through a carrier gateway or
    /// aggregator API (e.g., Twilio, Vonage, AWS SNS).
    /// </summary>
    SMS = 1,

    /// <summary>
    /// Mobile or web push notification delivered through a device push service
    /// (e.g., Firebase Cloud Messaging, Apple Push Notification Service).
    /// </summary>
    Push = 2
}
