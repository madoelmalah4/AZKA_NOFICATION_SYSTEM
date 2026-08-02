namespace Azka.NotificationPlatform.Application.DTOs;

/// <summary>
/// Dashboard response grouping notification delivery statistics by provider (FR-10).
/// </summary>
public sealed class ProviderSummaryDto
{
    /// <summary>Statistics for each registered notification provider.</summary>
    public required IReadOnlyList<ProviderStatDto> Providers { get; init; }
}

/// <summary>
/// Delivery statistics for a single notification provider.
/// </summary>
public sealed class ProviderStatDto
{
    /// <summary>Provider surrogate key.</summary>
    public required Guid ProviderId { get; init; }

    /// <summary>Human-readable provider name (e.g. "SendGrid", "Twilio", "FirebaseFCM").</summary>
    public required string ProviderName { get; init; }

    /// <summary>Delivery channel handled by this provider (e.g. "Email", "SMS", "Push").</summary>
    public required string Channel { get; init; }

    /// <summary>Whether the provider is currently active and eligible for dispatch.</summary>
    public required bool IsActive { get; init; }

    /// <summary>Total notifications routed through this provider's channel.</summary>
    public required int TotalNotifications { get; init; }

    /// <summary>Notifications that reached the terminal Delivered state.</summary>
    public required int Delivered { get; init; }

    /// <summary>Notifications that reached the terminal Failed state.</summary>
    public required int Failed { get; init; }

    /// <summary>Total delivery attempts made (including retries).</summary>
    public required int TotalAttempts { get; init; }

    /// <summary>
    /// Delivery success rate as a percentage of terminal notifications (Delivered / (Delivered + Failed) × 100).
    /// Returns 0 when no terminal notifications exist.
    /// </summary>
    public required double SuccessRate { get; init; }
}
