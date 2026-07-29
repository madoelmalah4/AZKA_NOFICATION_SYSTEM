using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.DTOs;

/// <summary>
/// Read-model / data-transfer object for a <see cref="Domain.Entities.Notification"/>.
/// Returned by query handlers; never passed as input to commands.
/// All properties are immutable via <see langword="init"/> accessors.
/// </summary>
public sealed record NotificationDto
{
    /// <summary>Platform-assigned surrogate key.</summary>
    public required Guid NotificationId { get; init; }

    /// <summary>Business type label (e.g., <c>"OrderConfirmation"</c>).</summary>
    public required string NotificationType { get; init; }

    /// <summary>Recipient delivery address.</summary>
    public required string Recipient { get; init; }

    /// <summary>Delivery channel.</summary>
    public required NotificationChannel Channel { get; init; }

    /// <summary>Message subject line (may be <see langword="null"/> for SMS).</summary>
    public string? Subject { get; init; }

    /// <summary>Rendered message body.</summary>
    public required string Body { get; init; }

    /// <summary>Current lifecycle status.</summary>
    public required NotificationStatus Status { get; init; }

    /// <summary>Caller-supplied idempotency key (FR-11).</summary>
    public required Guid CorrelationId { get; init; }

    /// <summary>UTC timestamp when the request was raised by the caller.</summary>
    public required DateTime RequestedAt { get; init; }

    /// <summary>External gateway-returned message ID (e.g., Firebase Message ID, SendGrid Message ID).</summary>
    public string? ExternalMessageId { get; init; }
}
