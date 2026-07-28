using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IDeliveryAttemptRepository"/> (FR-7).
/// </summary>
internal sealed class DeliveryAttemptRepository : IDeliveryAttemptRepository
{
    private readonly NotificationDbContext _dbContext;

    public DeliveryAttemptRepository(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeliveryAttempt?> GetByIdAsync(
        Guid attemptId, CancellationToken cancellationToken = default) =>
        await _dbContext.DeliveryAttempts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AttemptId == attemptId, cancellationToken);

    public async Task<IReadOnlyList<DeliveryAttempt>> GetByNotificationIdAsync(
        Guid notificationId, CancellationToken cancellationToken = default) =>
        await _dbContext.DeliveryAttempts
            .AsNoTracking()
            .Where(a => a.NotificationId == notificationId)
            .OrderBy(a => a.AttemptNumber)
            .ToListAsync(cancellationToken);

    public async Task<int> GetMaxAttemptNumberAsync(
        Guid notificationId, CancellationToken cancellationToken = default)
    {
        var max = await _dbContext.DeliveryAttempts
            .Where(a => a.NotificationId == notificationId)
            .Select(a => (int?)a.AttemptNumber)
            .MaxAsync(cancellationToken);

        return max ?? 0;
    }

    public async Task AddAsync(
        DeliveryAttempt attempt, CancellationToken cancellationToken = default) =>
        await _dbContext.DeliveryAttempts.AddAsync(attempt, cancellationToken);

    public void Update(DeliveryAttempt attempt) =>
        _dbContext.DeliveryAttempts.Update(attempt);
}
