namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Flat projection used by <see cref="INotificationProviderRepository.GetProviderStatisticsAsync"/>
/// to convey per-provider aggregated delivery statistics.
/// </summary>
public sealed class ProviderStatEntry
{
    /// <summary>Provider surrogate key.</summary>
    public required Guid ProviderId { get; init; }

    /// <summary>Human-readable provider name (e.g. "SendGrid", "Twilio").</summary>
    public required string ProviderName { get; init; }

    /// <summary>Delivery channel this provider handles.</summary>
    public required string Channel { get; init; }

    /// <summary>Whether the provider is currently active.</summary>
    public required bool IsActive { get; init; }

    /// <summary>Total notifications routed through this provider's channel.</summary>
    public required int TotalNotifications { get; init; }

    /// <summary>Count of notifications in a terminal Delivered state.</summary>
    public required int Delivered { get; init; }

    /// <summary>Count of notifications in a terminal Failed state.</summary>
    public required int Failed { get; init; }

    /// <summary>Count of total delivery attempts made for this provider's notifications.</summary>
    public required int TotalAttempts { get; init; }
}
