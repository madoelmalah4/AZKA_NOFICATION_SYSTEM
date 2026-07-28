namespace Azka.NotificationPlatform.Application.DTOs;

/// <summary>
/// Read-model / data-transfer object for a <see cref="Domain.Entities.DeliveryAttempt"/> (FR-7).
/// </summary>
public sealed record DeliveryAttemptDto
{
    /// <summary>Attempt surrogate key.</summary>
    public required Guid AttemptId { get; init; }

    /// <summary>Parent notification surrogate key.</summary>
    public required Guid NotificationId { get; init; }

    /// <summary>One-based retry ordinal.</summary>
    public required int AttemptNumber { get; init; }

    /// <summary>UTC timestamp the dispatch call was initiated.</summary>
    public required DateTime StartedAt { get; init; }

    /// <summary>UTC timestamp the dispatch call completed; <see langword="null"/> if still in-flight.</summary>
    public DateTime? CompletedAt { get; init; }

    /// <summary>Outcome label: <c>"Success"</c>, <c>"Failure"</c>, <c>"Timeout"</c>, or <see langword="null"/> if in-flight.</summary>
    public string? Result { get; init; }

    /// <summary>Raw provider response payload or error message.</summary>
    public string? ProviderResponse { get; init; }
}
