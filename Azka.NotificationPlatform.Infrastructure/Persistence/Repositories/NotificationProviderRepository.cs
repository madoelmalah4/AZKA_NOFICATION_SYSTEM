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
}
