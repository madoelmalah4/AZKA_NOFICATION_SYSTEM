using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.DTOs;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Dashboard.Queries;

/// <summary>
/// Handles <see cref="GetNotificationSummaryQuery"/>.
/// Aggregates counts by status and computes success/failure rates (FR-10).
/// </summary>
public sealed class GetNotificationSummaryQueryHandler
    : IRequestHandler<GetNotificationSummaryQuery, NotificationSummaryDto>
{
    private readonly INotificationRepository _repo;

    public GetNotificationSummaryQueryHandler(INotificationRepository repo) => _repo = repo;

    public async Task<NotificationSummaryDto> Handle(
        GetNotificationSummaryQuery request,
        CancellationToken cancellationToken)
    {
        // Status enum values: Pending=0, Queued=1, Processing=2, Delivered=3, Failed=4, Cancelled=5
        var counts = await _repo.GetStatusCountsAsync(cancellationToken);

        int pending    = counts.GetValueOrDefault(0);
        int queued     = counts.GetValueOrDefault(1);
        int processing = counts.GetValueOrDefault(2);
        int delivered  = counts.GetValueOrDefault(3);
        int failed     = counts.GetValueOrDefault(4);
        int cancelled  = counts.GetValueOrDefault(5);
        int total      = pending + queued + processing + delivered + failed + cancelled;

        int terminal   = delivered + failed;
        double successRate = terminal > 0 ? Math.Round((double)delivered / terminal * 100, 2) : 0;
        double failureRate = terminal > 0 ? Math.Round((double)failed    / terminal * 100, 2) : 0;

        return new NotificationSummaryDto
        {
            TotalNotifications = total,
            Pending            = pending,
            Queued             = queued,
            Processing         = processing,
            Delivered          = delivered,
            Failed             = failed,
            Cancelled          = cancelled,
            SuccessRate        = successRate,
            FailureRate        = failureRate
        };
    }
}
