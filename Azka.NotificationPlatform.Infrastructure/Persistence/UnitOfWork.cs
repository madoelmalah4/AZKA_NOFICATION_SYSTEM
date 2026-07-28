using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Infrastructure.Persistence;

namespace Azka.NotificationPlatform.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUnitOfWork"/>.
/// Wraps <see cref="NotificationDbContext.SaveChangesAsync"/> so the Application
/// Layer never holds a direct reference to the EF Core context.
/// </summary>
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly NotificationDbContext _dbContext;

    public UnitOfWork(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}
