using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Domain.Enums;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Dashboard.Queries;

/// <summary>
/// Handles <see cref="GetChannelSummaryQuery"/>.
/// Computes per-channel metrics (Email, SMS, Push) from database aggregated values (FR-10).
/// </summary>
public sealed class GetChannelSummaryQueryHandler
    : IRequestHandler<GetChannelSummaryQuery, ChannelSummaryDto>
{
    private readonly INotificationRepository _repo;

    public GetChannelSummaryQueryHandler(INotificationRepository repo) => _repo = repo;

    public async Task<ChannelSummaryDto> Handle(
        GetChannelSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var rawData = await _repo.GetCountByChannelAndStatusAsync(cancellationToken);

        return new ChannelSummaryDto
        {
            Email = GetChannelStat(rawData, 0, "Email"),
            Sms = GetChannelStat(rawData, 1, "SMS"),
            Push = GetChannelStat(rawData, 2, "Push")
        };
    }

    private static ChannelStatDto GetChannelStat(Dictionary<(int Channel, int Status), int> data, int channelVal, string name)
    {
        // Status values: Pending=0, Queued=1, Processing=2, Delivered=3, Failed=4, Cancelled=5
        int pending = data.GetValueOrDefault((channelVal, 0)) + 
                      data.GetValueOrDefault((channelVal, 1)) + 
                      data.GetValueOrDefault((channelVal, 2)); // Combined non-terminal queue states for simplified dashboard pending display
        
        int delivered = data.GetValueOrDefault((channelVal, 3));
        int failed = data.GetValueOrDefault((channelVal, 4));
        int cancelled = data.GetValueOrDefault((channelVal, 5));

        int total = pending + delivered + failed + cancelled;
        int terminal = delivered + failed;
        double successRate = terminal > 0 ? Math.Round((double)delivered / terminal * 100, 2) : 0;

        return new ChannelStatDto
        {
            Channel = name,
            Total = total,
            Delivered = delivered,
            Failed = failed,
            Pending = pending,
            SuccessRate = successRate
        };
    }
}
