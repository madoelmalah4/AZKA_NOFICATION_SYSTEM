namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Transactional boundary abstraction. The Infrastructure Layer implements this over an
/// EF Core <c>DbContext</c>, but the Application Layer only sees this interface.
/// All repository operations within a single command handler share the same unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all pending changes tracked within the current unit of work to the
    /// durable store atomically.
    /// </summary>
    /// <param name="cancellationToken">Propagates cancellation from the caller.</param>
    /// <returns>The number of state-change records written to the store.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
