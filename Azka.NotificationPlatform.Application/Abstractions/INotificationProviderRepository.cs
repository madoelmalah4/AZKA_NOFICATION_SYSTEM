using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Repository contract for <see cref="NotificationProvider"/> persistence.
/// </summary>
public interface INotificationProviderRepository
{
    /// <summary>Retrieves a provider by its surrogate key.</summary>
    Task<NotificationProvider?> GetByIdAsync(Guid providerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all active providers that support the specified channel.
    /// The application layer selects the appropriate provider from this list.
    /// </summary>
    Task<IReadOnlyList<NotificationProvider>> GetActiveByChannelAsync(
        NotificationChannel channel,
        CancellationToken cancellationToken = default);

    /// <summary>Returns all providers regardless of activation state.</summary>
    Task<IReadOnlyList<NotificationProvider>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Persists a new <see cref="NotificationProvider"/>.</summary>
    Task AddAsync(NotificationProvider provider, CancellationToken cancellationToken = default);

    /// <summary>Marks a provider as modified in the unit of work.</summary>
    void Update(NotificationProvider provider);
}
