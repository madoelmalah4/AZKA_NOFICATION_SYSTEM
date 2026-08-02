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

    public void Remove(Notification notification) =>
        _dbContext.Notifications.Remove(notification);

    public async Task<Dictionary<int, int>> GetStatusCountsAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Notifications
            .GroupBy(n => (int)n.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken);

    public async Task<Dictionary<(int Channel, int Status), int>> GetCountByChannelAndStatusAsync(CancellationToken cancellationToken = default) =>
        (await _dbContext.Notifications
            .GroupBy(n => new { Channel = (int)n.Channel, Status = (int)n.Status })
            .Select(g => new { g.Key.Channel, g.Key.Status, Count = g.Count() })
            .ToListAsync(cancellationToken))
            .ToDictionary(x => (x.Channel, x.Status), x => x.Count);

    public async Task<Application.DTOs.PagedResult<Application.DTOs.NotificationDto>> SearchAsync(
        Application.Features.Notifications.Queries.SearchNotificationsQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

        var queryable = _dbContext.Notifications.AsNoTracking().AsQueryable();

        if (query.NotificationId.HasValue && query.NotificationId.Value != Guid.Empty)
        {
            queryable = queryable.Where(n => n.NotificationId == query.NotificationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Recipient))
        {
            var recipientClean = query.Recipient.Trim();
            queryable = queryable.Where(n => n.Recipient.Contains(recipientClean));
        }

        if (query.Channel.HasValue)
        {
            queryable = queryable.Where(n => n.Channel == query.Channel.Value);
        }

        if (query.Status.HasValue)
        {
            queryable = queryable.Where(n => n.Status == query.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.NotificationType))
        {
            var typeClean = query.NotificationType.Trim();
            queryable = queryable.Where(n => n.NotificationType.Contains(typeClean));
        }

        if (query.FromDate.HasValue)
        {
            queryable = queryable.Where(n => n.RequestedAt >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            queryable = queryable.Where(n => n.RequestedAt <= query.ToDate.Value);
        }

        if (query.CorrelationId.HasValue && query.CorrelationId.Value != Guid.Empty)
        {
            queryable = queryable.Where(n => n.CorrelationId == query.CorrelationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.ApplicationName))
        {
            var appClean = query.ApplicationName.Trim();
            queryable = queryable.Where(n => n.ApplicationName != null && n.ApplicationName.Contains(appClean));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

        var items = await queryable
            .OrderByDescending(n => n.RequestedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new Application.DTOs.NotificationDto
            {
                NotificationId = n.NotificationId,
                NotificationType = n.NotificationType,
                Recipient = n.Recipient,
                Channel = n.Channel,
                Subject = n.Subject,
                Body = n.Body,
                Status = n.Status,
                CorrelationId = n.CorrelationId,
                RequestedAt = n.RequestedAt,
                ApplicationName = n.ApplicationName
            })
            .ToListAsync(cancellationToken);

        return new Application.DTOs.PagedResult<Application.DTOs.NotificationDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}
