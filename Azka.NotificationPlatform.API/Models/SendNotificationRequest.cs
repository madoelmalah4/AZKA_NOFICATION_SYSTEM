using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.API.Models;

/// <summary>
/// Request body for submitting a notification.
/// </summary>
public sealed class SendNotificationRequest
{
    /// <summary>
    /// The template to send. Choose one of the pre-built templates below:
    /// <br/>• <b>UserRegistration</b>  — Welcome email/SMS sent on new account creation
    /// <br/>• <b>PasswordReset</b>     — Password reset instructions
    /// <br/>• <b>OrderConfirmation</b> — Order receipt confirmation
    /// <br/>• <b>SystemAlert</b>       — Critical system alert (Email only)
    /// </summary>
    /// <example>UserRegistration</example>
    public required string NotificationType { get; init; }

    /// <summary>
    /// Recipient address.
    /// <br/>• Channel <b>0 (Email)</b>: e.g. <c>user@example.com</c>
    /// <br/>• Channel <b>1 (SMS)</b>: E.164 number e.g. <c>+9665XXXXXXXX</c>
    /// <br/>• Channel <b>2 (Push)</b>: FCM device token
    /// </summary>
    /// <example>user@example.com</example>
    public required string Recipient { get; init; }

    /// <summary>
    /// Delivery channel — <b>0</b> = Email · <b>1</b> = SMS · <b>2</b> = Push
    /// </summary>
    /// <example>0</example>
    public required NotificationChannel Channel { get; init; }

    /// <summary>
    /// Optional upstream application or system name submitting the notification.
    /// </summary>
    /// <example>PaymentSystem</example>
    public string? ApplicationName { get; init; }
}
