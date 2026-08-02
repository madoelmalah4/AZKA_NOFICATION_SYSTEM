using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Commands;

/// <summary>
/// Handles <see cref="SendNotificationCommand"/>.
/// Enforces duplicate prevention via CorrelationId, persists the notification as Pending, 
/// enqueues it to the high-throughput memory queue, and returns immediately (non-blocking).
/// </summary>
public sealed class SendNotificationCommandHandler
    : IRequestHandler<SendNotificationCommand, NotificationDto>
{
    private readonly INotificationRepository _notificationRepo;
    private readonly INotificationHistoryRepository _historyRepo;
    private readonly ITemplateRendererService _templateRenderer;
    private readonly INotificationQueue _notificationQueue;
    private readonly IUnitOfWork _unitOfWork;

    public SendNotificationCommandHandler(
        INotificationRepository notificationRepo,
        INotificationHistoryRepository historyRepo,
        ITemplateRendererService templateRenderer,
        INotificationQueue notificationQueue,
        IUnitOfWork unitOfWork)
    {
        _notificationRepo = notificationRepo;
        _historyRepo = historyRepo;
        _templateRenderer = templateRenderer;
        _notificationQueue = notificationQueue;
        _unitOfWork = unitOfWork;
    }

    public async Task<NotificationDto> Handle(
        SendNotificationCommand request,
        CancellationToken cancellationToken)
    {
        // ── FR-11: Idempotency (Duplicate Prevention) Check ──
        var existing = await _notificationRepo.GetByCorrelationIdAsync(request.CorrelationId, cancellationToken);
        if (existing is not null)
        {
            return MapToDto(existing);
        }

        // ── FR-3: Template Resolution and Rendering ──
        var (renderedSubject, renderedBody) = await _templateRenderer.RenderAsync(
            request.NotificationType,
            request.Channel,
            request.Language,
            request.TemplateData,
            cancellationToken);

        // ── Build and persist the Notification aggregate ──
        var notificationId = Guid.NewGuid();
        var notification = new Notification(
            notificationId: notificationId,
            notificationType: request.NotificationType,
            recipient: request.Recipient,
            channel: request.Channel,
            body: renderedBody,
            correlationId: request.CorrelationId,
            requestedAt: request.RequestedAt,
            subject: renderedSubject,
            applicationName: request.ApplicationName);

        await _notificationRepo.AddAsync(notification, cancellationToken);

        // Record the initial Pending state transition
        await _historyRepo.AddAsync(
            new NotificationHistory(
                historyId: Guid.NewGuid(),
                notificationId: notificationId,
                status: NotificationStatus.Pending,
                changedAt: DateTime.UtcNow,
                remarks: "Notification intake accepted."),
            cancellationToken);

        // Save to database atomically
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ── Transition Pending → Queued before enqueuing (worker expects Queued status) ──
        notification.MarkAsQueued();
        _notificationRepo.Update(notification);
        await _historyRepo.AddAsync(
            new NotificationHistory(
                historyId: Guid.NewGuid(),
                notificationId: notificationId,
                status: NotificationStatus.Queued,
                changedAt: DateTime.UtcNow,
                remarks: "Notification accepted and queued for dispatch."),
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ── FR-4: Enqueue notification Guid to high-throughput memory queue (Non-blocking) ──
        await _notificationQueue.EnqueueAsync(notificationId);

        return MapToDto(notification);
    }

    private static NotificationDto MapToDto(Notification n) => new()
    {
        NotificationId = n.NotificationId,
        NotificationType = n.NotificationType,
        Recipient = n.Recipient,
        Channel = n.Channel,
        Subject = n.Subject,
        Body = n.Body,
        Status = n.Status,
        CorrelationId = n.CorrelationId,
        RequestedAt = n.RequestedAt,
        ApplicationName = n.ApplicationName
    };
}
