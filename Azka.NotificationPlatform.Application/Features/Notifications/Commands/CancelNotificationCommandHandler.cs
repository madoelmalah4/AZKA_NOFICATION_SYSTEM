using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Commands;

/// <summary>
/// Handles <see cref="CancelNotificationCommand"/>.
/// Returns <see langword="true"/> on success, <see langword="false"/> if the
/// notification was not found or was already in a terminal state.
/// </summary>
public sealed class CancelNotificationCommandHandler
    : IRequestHandler<CancelNotificationCommand, bool>
{
    private readonly INotificationRepository        _notificationRepo;
    private readonly INotificationHistoryRepository _historyRepo;
    private readonly IUnitOfWork                    _unitOfWork;

    public CancelNotificationCommandHandler(
        INotificationRepository        notificationRepo,
        INotificationHistoryRepository historyRepo,
        IUnitOfWork                    unitOfWork)
    {
        _notificationRepo = notificationRepo;
        _historyRepo      = historyRepo;
        _unitOfWork       = unitOfWork;
    }

    public async Task<bool> Handle(
        CancelNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await _notificationRepo
            .GetByIdAsync(request.NotificationId, cancellationToken);

        if (notification is null)
            return false;

        try
        {
            notification.Cancel();
        }
        catch (InvalidOperationException)
        {
            // Already in a terminal state — treat as a no-op and return false.
            return false;
        }

        _notificationRepo.Update(notification);

        await _historyRepo.AddAsync(
            new NotificationHistory(
                historyId:      Guid.NewGuid(),
                notificationId: notification.NotificationId,
                status:         NotificationStatus.Cancelled,
                changedAt:      DateTime.UtcNow,
                remarks:        request.Reason),
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
