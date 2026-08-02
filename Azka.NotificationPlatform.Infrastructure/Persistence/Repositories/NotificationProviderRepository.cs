using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="INotificationProviderRepository"/>.
/// </summary>
internal sealed class NotificationProviderRepository : INotificationProviderRepository
{
    private readonly NotificationDbContext _dbContext;

    public NotificationProviderRepository(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<NotificationProvider?> GetByIdAsync(
        Guid providerId, CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationProviders
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProviderId == providerId, cancellationToken);

    public async Task<IReadOnlyList<NotificationProvider>> GetActiveByChannelAsync(
        NotificationChannel channel, CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationProviders
            .AsNoTracking()
            .Where(p => p.Channel == channel && p.IsActive)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<NotificationProvider>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationProviders
            .AsNoTracking()
            .OrderBy(p => p.ProviderName)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(
        NotificationProvider provider, CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationProviders.AddAsync(provider, cancellationToken);

    public void Update(NotificationProvider provider) =>
        _dbContext.NotificationProviders.Update(provider);

    public async Task<IReadOnlyList<ProviderStatEntry>> GetProviderStatisticsAsync(
        CancellationToken cancellationToken = default)
    {
        // Status enum values: Delivered=3, Failed=4
        const int deliveredStatus = (int)NotificationStatus.Delivered;
        const int failedStatus    = (int)NotificationStatus.Failed;

        // Load all providers
        var providers = await _dbContext.NotificationProviders
            .AsNoTracking()
            .OrderBy(p => p.ProviderName)
            .ToListAsync(cancellationToken);

        // Aggregate notifications by channel and status in one query
        var notifCounts = await _dbContext.Notifications
            .AsNoTracking()
            .GroupBy(n => new { Channel = (int)n.Channel, Status = (int)n.Status })
            .Select(g => new { g.Key.Channel, g.Key.Status, Count = g.Count() })
            .ToListAsync(cancellationToken);

        // Count total delivery attempts per channel (via notification join)
        var attemptsByChannel = await _dbContext.DeliveryAttempts
            .AsNoTracking()
            .Join(_dbContext.Notifications,
                  a => a.NotificationId,
                  n => n.NotificationId,
                  (a, n) => new { Channel = (int)n.Channel })
            .GroupBy(x => x.Channel)
            .Select(g => new { Channel = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        // Project into ProviderStatEntry — one entry per provider
        var results = providers.Select(p =>
        {
            int ch = (int)p.Channel;

            int total     = notifCounts.Where(x => x.Channel == ch).Sum(x => x.Count);
            int delivered = notifCounts.FirstOrDefault(x => x.Channel == ch && x.Status == deliveredStatus)?.Count ?? 0;
            int failed    = notifCounts.FirstOrDefault(x => x.Channel == ch && x.Status == failedStatus)?.Count    ?? 0;
            int attempts  = attemptsByChannel.FirstOrDefault(x => x.Channel == ch)?.Count                         ?? 0;

            return new ProviderStatEntry
            {
                ProviderId         = p.ProviderId,
                ProviderName       = p.ProviderName,
                Channel            = p.Channel.ToString(),
                IsActive           = p.IsActive,
                TotalNotifications = total,
                Delivered          = delivered,
                Failed             = failed,
                TotalAttempts      = attempts
            };
        }).ToList();

        return results;
    }
}
