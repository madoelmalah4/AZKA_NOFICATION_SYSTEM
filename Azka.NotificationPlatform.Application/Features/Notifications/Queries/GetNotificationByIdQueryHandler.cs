using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Domain.Entities;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Queries;

/// <summary>
/// Handles <see cref="GetNotificationByIdQuery"/>.
/// Returns <see langword="null"/> if no notification exists for the given ID.
/// </summary>
public sealed class GetNotificationByIdQueryHandler
    : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
{
    private readonly INotificationRepository _notificationRepo;

    public GetNotificationByIdQueryHandler(INotificationRepository notificationRepo)
    {
        _notificationRepo = notificationRepo;
    }

    public async Task<NotificationDto?> Handle(
        GetNotificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var notification = await _notificationRepo
            .GetByIdAsync(request.NotificationId, cancellationToken);

        return notification is null ? null : MapToDto(notification);
    }

    private static NotificationDto MapToDto(Notification n) => new()
    {
        NotificationId   = n.NotificationId,
        NotificationType = n.NotificationType,
        Recipient        = n.Recipient,
        Channel          = n.Channel,
        Subject          = n.Subject,
        Body             = n.Body,
        Status           = n.Status,
        CorrelationId    = n.CorrelationId,
        RequestedAt      = n.RequestedAt
    };
}
