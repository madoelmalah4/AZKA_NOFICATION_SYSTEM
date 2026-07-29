namespace Azka.NotificationPlatform.Application.DTOs;

/// <summary>Per-channel notification statistics for the dashboard (FR-10).</summary>
public sealed class ChannelSummaryDto
{
    public ChannelStatDto Email { get; init; } = new();
    public ChannelStatDto Sms { get; init; } = new();
    public ChannelStatDto Push { get; init; } = new();
}

public sealed class ChannelStatDto
{
    public string Channel { get; init; } = string.Empty;
    public int Total { get; init; }
    public int Delivered { get; init; }
    public int Failed { get; init; }
    public int Pending { get; init; }
    public double SuccessRate { get; init; }
}
