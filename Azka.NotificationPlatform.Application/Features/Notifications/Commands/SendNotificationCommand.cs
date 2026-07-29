using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Domain.Enums;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Commands;

/// <summary>
/// CQRS command: submit a new notification request to the Azka platform.
/// Dispatched by the API controller via MediatR.
/// </summary>
/// <remarks>
/// The <see cref="CorrelationId"/> field is the caller-owned idempotency key (FR-11).
/// If a notification with the same <see cref="CorrelationId"/> already exists, the handler
/// returns the existing <see cref="NotificationDto"/> without creating a duplicate.
/// </remarks>
public sealed record SendNotificationCommand : IRequest<NotificationDto>
{
    /// <summary>
    /// Caller-supplied idempotency key. Must be a non-empty GUID unique to this
    /// business event. The platform uses it to deduplicate retried requests (FR-11).
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public Guid CorrelationId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Business type label used to resolve the matching
    /// <see cref="Domain.Entities.NotificationTemplate"/> (FR-3).
    /// </summary>
    public required string NotificationType { get; init; }

    /// <summary>
    /// Delivery address of the intended recipient, appropriate for the chosen
    /// <see cref="Channel"/> (email address, E.164 number, or push token).
    /// </summary>
    public required string Recipient { get; init; }

    /// <summary>Delivery channel for this notification.</summary>
    public required NotificationChannel Channel { get; init; }

    [System.Text.Json.Serialization.JsonIgnore]
    public string Language { get; init; } = "en-US";

    [System.Text.Json.Serialization.JsonIgnore]
    public IReadOnlyDictionary<string, string> TemplateData { get; init; }
        = new Dictionary<string, string>();

    /// <summary>UTC timestamp — auto-set server-side.</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public DateTime RequestedAt { get; init; } = DateTime.UtcNow;
}
