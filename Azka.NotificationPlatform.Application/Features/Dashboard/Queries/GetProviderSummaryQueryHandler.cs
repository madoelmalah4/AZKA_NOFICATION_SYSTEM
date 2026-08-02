using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.DTOs;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Dashboard.Queries;

/// <summary>
/// Handles <see cref="GetProviderSummaryQuery"/>.
/// Maps raw <see cref="ProviderStatEntry"/> projections from the repository into
/// <see cref="ProviderSummaryDto"/>, computing the derived SuccessRate field (FR-10).
/// </summary>
public sealed class GetProviderSummaryQueryHandler
    : IRequestHandler<GetProviderSummaryQuery, ProviderSummaryDto>
{
    private readonly INotificationProviderRepository _providerRepo;

    public GetProviderSummaryQueryHandler(INotificationProviderRepository providerRepo)
        => _providerRepo = providerRepo;

    public async Task<ProviderSummaryDto> Handle(
        GetProviderSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var entries = await _providerRepo.GetProviderStatisticsAsync(cancellationToken);

        var stats = entries.Select(e =>
        {
            int terminal    = e.Delivered + e.Failed;
            double successRate = terminal > 0
                ? Math.Round((double)e.Delivered / terminal * 100, 2)
                : 0;

            return new ProviderStatDto
            {
                ProviderId         = e.ProviderId,
                ProviderName       = e.ProviderName,
                Channel            = e.Channel,
                IsActive           = e.IsActive,
                TotalNotifications = e.TotalNotifications,
                Delivered          = e.Delivered,
                Failed             = e.Failed,
                TotalAttempts      = e.TotalAttempts,
                SuccessRate        = successRate
            };
        }).ToList();

        return new ProviderSummaryDto { Providers = stats };
    }
}
