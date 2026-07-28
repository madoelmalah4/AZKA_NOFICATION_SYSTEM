using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="INotificationTemplateRepository"/> (FR-3).
/// </summary>
internal sealed class NotificationTemplateRepository : INotificationTemplateRepository
{
    private readonly NotificationDbContext _dbContext;

    public NotificationTemplateRepository(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<NotificationTemplate?> GetByIdAsync(
        Guid templateId, CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TemplateId == templateId, cancellationToken);

    public async Task<NotificationTemplate?> GetActiveTemplateAsync(
        string notificationType,
        NotificationChannel channel,
        string language,
        CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationTemplates
            .AsNoTracking()
            .Where(t => t.TemplateName.StartsWith(notificationType)
                     && t.Channel   == channel
                     && t.Language  == language
                     && t.Status    == "Active")
            .OrderByDescending(t => t.Version)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<NotificationTemplate>> GetAllAsync(
        NotificationChannel? channel = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.NotificationTemplates.AsNoTracking();

        if (channel.HasValue)
            query = query.Where(t => t.Channel == channel.Value);

        return await query
            .OrderBy(t => t.TemplateName)
            .ThenByDescending(t => t.Version)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        NotificationTemplate template, CancellationToken cancellationToken = default) =>
        await _dbContext.NotificationTemplates.AddAsync(template, cancellationToken);

    public void Update(NotificationTemplate template) =>
        _dbContext.NotificationTemplates.Update(template);
}
