using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="INotificationRepository"/>.
/// </summary>
internal sealed class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _dbContext;

    public NotificationRepository(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Notification?> GetByIdAsync(
        Guid notificationId, CancellationToken cancellationToken = default) =>
        await _dbContext.Notifications
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.NotificationId == notificationId, cancellationToken);

    public async Task<Notification?> GetByCorrelationIdAsync(
        Guid correlationId, CancellationToken cancellationToken = default) =>
        await _dbContext.Notifications
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.CorrelationId == correlationId, cancellationToken);

    public async Task<IReadOnlyList<Notification>> GetByRecipientAsync(
        string recipient, CancellationToken cancellationToken = default) =>
        await _dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.Recipient == recipient)
            .OrderByDescending(n => n.RequestedAt)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Notification>> GetByStatusAsync(
        NotificationStatus status, CancellationToken cancellationToken = default) =>
        await _dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.Status == status)
            .OrderBy(n => n.RequestedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(
        Notification notification, CancellationToken cancellationToken = default) =>
        await _dbContext.Notifications.AddAsync(notification, cancellationToken);

    public void Update(Notification notification) =>
        _dbContext.Notifications.Update(notification);
}
