using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.DTOs;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Queries;

/// <summary>
/// CQRS query handler for searching notifications (FR-9).
/// </summary>
public sealed class SearchNotificationsQueryHandler
    : IRequestHandler<SearchNotificationsQuery, PagedResult<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;

    public SearchNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<PagedResult<NotificationDto>> Handle(
        SearchNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        return await _notificationRepository.SearchAsync(request, cancellationToken);
    }
}
