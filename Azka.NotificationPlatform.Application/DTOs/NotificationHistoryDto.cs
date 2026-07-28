using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.DTOs;

/// <summary>
/// Read-model / data-transfer object for a <see cref="Domain.Entities.NotificationHistory"/>
/// audit record (FR-7).
/// </summary>
public sealed record NotificationHistoryDto
{
    /// <summary>History entry surrogate key.</summary>
    public required Guid HistoryId { get; init; }

    /// <summary>Parent notification surrogate key.</summary>
    public required Guid NotificationId { get; init; }

    /// <summary>The destination status recorded at this transition.</summary>
    public required NotificationStatus Status { get; init; }

    /// <summary>UTC timestamp when the transition occurred.</summary>
    public required DateTime ChangedAt { get; init; }

    /// <summary>Optional free-text annotation explaining the transition.</summary>
    public string? Remarks { get; init; }
}
