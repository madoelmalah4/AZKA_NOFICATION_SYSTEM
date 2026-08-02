using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Domain.Enums;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Queries;

/// <summary>
/// CQRS query: Search and filter notifications with database-level pagination (FR-9).
/// All filter parameters are optional and can be combined in any combination.
/// </summary>
public sealed record SearchNotificationsQuery : IRequest<PagedResult<NotificationDto>>
{
    /// <summary>Filter by exact notification surrogate key.</summary>
    public Guid? NotificationId { get; init; }

    /// <summary>Filter by recipient delivery address (email, phone number, or token).</summary>
    public string? Recipient { get; init; }

    /// <summary>Filter by delivery channel: 0 = Email, 1 = SMS, 2 = Push.</summary>
    public NotificationChannel? Channel { get; init; }

    /// <summary>Filter by notification status: 0 = Pending, 1 = Queued, 2 = Processing, 3 = Delivered, 4 = Failed, 5 = Cancelled.</summary>
    public NotificationStatus? Status { get; init; }

    /// <summary>Filter by business type label (e.g. "UserRegistration", "OrderConfirmation").</summary>
    public string? NotificationType { get; init; }

    /// <summary>Filter by request date range start (inclusive UTC timestamp).</summary>
    public DateTime? FromDate { get; init; }

    /// <summary>Filter by request date range end (inclusive UTC timestamp).</summary>
    public DateTime? ToDate { get; init; }

    /// <summary>Filter by caller-supplied correlation / idempotency ID.</summary>
    public Guid? CorrelationId { get; init; }

    /// <summary>Filter by upstream application name (e.g. "PaymentSystem").</summary>
    public string? ApplicationName { get; init; }

    /// <summary>Page number for pagination (1-based index, default 1).</summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>Number of items per page (default 20).</summary>
    public int PageSize { get; init; } = 20;
}
