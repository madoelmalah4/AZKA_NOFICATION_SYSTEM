using Azka.NotificationPlatform.Application.DTOs;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Queries;

/// <summary>
/// CQRS query: retrieve the full status-transition audit trail for a notification (FR-7).
/// </summary>
public sealed record GetNotificationHistoryQuery : IRequest<IReadOnlyList<NotificationHistoryDto>>
{
    /// <summary>The parent notification whose history is requested.</summary>
    public required Guid NotificationId { get; init; }
}
