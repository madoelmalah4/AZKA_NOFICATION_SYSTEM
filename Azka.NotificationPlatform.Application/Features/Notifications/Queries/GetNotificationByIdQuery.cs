using Azka.NotificationPlatform.Application.DTOs;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Queries;

/// <summary>
/// CQRS query: fetch a single notification by its platform-assigned surrogate key.
/// </summary>
public sealed record GetNotificationByIdQuery : IRequest<NotificationDto?>
{
    /// <summary>The platform-assigned <see cref="Domain.Entities.Notification.NotificationId"/>.</summary>
    public required Guid NotificationId { get; init; }
}
