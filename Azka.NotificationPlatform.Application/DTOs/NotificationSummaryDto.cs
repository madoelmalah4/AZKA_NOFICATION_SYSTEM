namespace Azka.NotificationPlatform.Application.DTOs;

/// <summary>Overall platform notification statistics for the dashboard (FR-10).</summary>
public sealed class NotificationSummaryDto
{
    public int TotalNotifications { get; init; }
    public int Pending { get; init; }
    public int Queued { get; init; }
    public int Processing { get; init; }
    public int Delivered { get; init; }
    public int Failed { get; init; }
    public int Cancelled { get; init; }

    /// <summary>Percentage of delivered out of all terminal notifications (Delivered + Failed).</summary>
    public double SuccessRate { get; init; }

    /// <summary>Percentage of failed out of all terminal notifications (Delivered + Failed).</summary>
    public double FailureRate { get; init; }
}
