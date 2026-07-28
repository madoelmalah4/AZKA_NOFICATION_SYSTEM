using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="INotificationHistoryRepository"/> (FR-7).
/// Exposes only <c>AddAsync</c> and read operations — no update or delete.
/// </summary>
internal sealed class NotificationHistoryRepository : INotificationHistoryRepository
{
    private readonly NotificationDbContext _dbContext;

    public NotificationHistoryRepository(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<NotificationHistory>> GetByNotificationIdAsync(
        Guid notificationId, CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationHistories
            .AsNoTracking()
            .Where(h => h.NotificationId == notificationId)
            .OrderBy(h => h.ChangedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(
        NotificationHistory history, CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationHistories.AddAsync(history, cancellationToken);
}
