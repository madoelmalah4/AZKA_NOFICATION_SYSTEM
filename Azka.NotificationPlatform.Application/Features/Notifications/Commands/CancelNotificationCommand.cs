using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Commands;

/// <summary>
/// CQRS command: cancel an existing notification that has not yet reached a terminal state.
/// </summary>
public sealed record CancelNotificationCommand : IRequest<bool>
{
    /// <summary>Platform-assigned surrogate key of the notification to cancel.</summary>
    public required Guid NotificationId { get; init; }

    /// <summary>Optional human-readable reason for the cancellation, stored in history.</summary>
    public string? Reason { get; init; }
}
