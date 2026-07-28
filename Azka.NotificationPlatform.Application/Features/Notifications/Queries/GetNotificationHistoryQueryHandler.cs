using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Domain.Entities;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Queries;

/// <summary>
/// Handles <see cref="GetNotificationHistoryQuery"/>.
/// Returns the full ordered history list; an empty list if the notification has
/// no history entries (does not distinguish between "not found" and "no history").
/// </summary>
public sealed class GetNotificationHistoryQueryHandler
    : IRequestHandler<GetNotificationHistoryQuery, IReadOnlyList<NotificationHistoryDto>>
{
    private readonly INotificationHistoryRepository _historyRepo;

    public GetNotificationHistoryQueryHandler(INotificationHistoryRepository historyRepo)
    {
        _historyRepo = historyRepo;
    }

    public async Task<IReadOnlyList<NotificationHistoryDto>> Handle(
        GetNotificationHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var records = await _historyRepo.GetByNotificationIdAsync(
            request.NotificationId, cancellationToken);

        return records
            .Select(MapToDto)
            .ToList()
            .AsReadOnly();
    }

    private static NotificationHistoryDto MapToDto(NotificationHistory h) => new()
    {
        HistoryId      = h.HistoryId,
        NotificationId = h.NotificationId,
        Status         = h.Status,
        ChangedAt      = h.ChangedAt,
        Remarks        = h.Remarks
    };
}
