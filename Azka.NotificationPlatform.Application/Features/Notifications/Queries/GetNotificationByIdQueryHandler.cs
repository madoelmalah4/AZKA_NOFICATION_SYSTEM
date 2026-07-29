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
    private readonly IDeliveryAttemptRepository _attemptRepo;

    public GetNotificationByIdQueryHandler(
        INotificationRepository notificationRepo,
        IDeliveryAttemptRepository attemptRepo)
    {
        _notificationRepo = notificationRepo;
        _attemptRepo = attemptRepo;
    }

    public async Task<NotificationDto?> Handle(
        GetNotificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var notification = await _notificationRepo
            .GetByIdAsync(request.NotificationId, cancellationToken);

        if (notification is null) return null;

        string? externalMessageId = null;
        var attempts = await _attemptRepo.GetByNotificationIdAsync(notification.NotificationId, cancellationToken);
        var latestAttempt = attempts.OrderByDescending(a => a.AttemptNumber).FirstOrDefault();
        if (latestAttempt != null && !string.IsNullOrEmpty(latestAttempt.ProviderResponse))
        {
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(latestAttempt.ProviderResponse);
                if (doc.RootElement.TryGetProperty("messageId", out var prop))
                {
                    externalMessageId = prop.GetString();
                }
            }
            catch
            {
                // Fallback if not valid JSON or missing property
            }
        }

        return MapToDto(notification, externalMessageId);
    }

    private static NotificationDto MapToDto(Notification n, string? externalMessageId) => new()
    {
        NotificationId   = n.NotificationId,
        NotificationType = n.NotificationType,
        Recipient        = n.Recipient,
        Channel          = n.Channel,
        Subject          = n.Subject,
        Body             = n.Body,
        Status           = n.Status,
        CorrelationId    = n.CorrelationId,
        RequestedAt      = n.RequestedAt,
        ExternalMessageId = externalMessageId
    };
}
