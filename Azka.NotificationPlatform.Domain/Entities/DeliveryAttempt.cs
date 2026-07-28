namespace Azka.NotificationPlatform.Domain.Entities;

/// <summary>
/// Records a single attempt by the platform to deliver a <see cref="Notification"/>
/// through a downstream provider (FR-7 Delivery Status tracking).
/// </summary>
/// <remarks>
/// A <see cref="Notification"/> may accumulate multiple <see cref="DeliveryAttempt"/>
/// records when retry logic is applied (e.g., transient provider failures, rate limits).
/// Each attempt is immutable once completed — the application layer appends new records
/// rather than mutating existing ones. This preserves a complete audit trail.
///
/// Relationship:
///   Notification 1 ──────────── * DeliveryAttempt
/// </remarks>
public sealed class DeliveryAttempt
{
    /// <summary>
    /// Primary surrogate key for this delivery attempt record.
    /// </summary>
    public Guid AttemptId { get; init; }

    /// <summary>
    /// Foreign key reference to the parent <see cref="Notification.NotificationId"/>.
    /// </summary>
    public Guid NotificationId { get; init; }

    /// <summary>
    /// One-based ordinal indicating which attempt number this is for the parent
    /// notification. Attempt 1 is the initial dispatch; subsequent values indicate
    /// retries. Must be ≥ 1.
    /// </summary>
    public int AttemptNumber { get; init; }

    /// <summary>
    /// UTC timestamp at which the dispatch call to the downstream provider was initiated.
    /// </summary>
    public DateTime StartedAt { get; init; }

    /// <summary>
    /// UTC timestamp at which the provider call completed (either successfully or with
    /// an error). <see langword="null"/> if the attempt is still in-flight.
    /// </summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>
    /// Human-readable outcome of the attempt.
    /// Conventional values: <c>"Success"</c>, <c>"Failure"</c>, <c>"Timeout"</c>.
    /// <see langword="null"/> if the attempt has not yet completed.
    /// </summary>
    public string? Result { get; private set; }

    /// <summary>
    /// The raw response payload or error message returned by the downstream provider
    /// (e.g., an HTTP status code + body, or a provider-specific error code).
    /// Stored verbatim for diagnostic purposes. <see langword="null"/> if not yet
    /// available or if the provider returned no meaningful response.
    /// </summary>
    public string? ProviderResponse { get; private set; }

    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>
    /// Initialises a new in-flight <see cref="DeliveryAttempt"/>. The attempt is
    /// considered open until <see cref="Complete"/> is called.
    /// </summary>
    /// <param name="attemptId">Platform-generated surrogate key.</param>
    /// <param name="notificationId">Parent notification reference.</param>
    /// <param name="attemptNumber">One-based ordinal (≥ 1).</param>
    /// <param name="startedAt">UTC timestamp the call was initiated.</param>
    public DeliveryAttempt(
        Guid attemptId,
        Guid notificationId,
        int attemptNumber,
        DateTime startedAt)
    {
        if (attemptId      == Guid.Empty) throw new ArgumentException("AttemptId must not be an empty GUID.",      nameof(attemptId));
        if (notificationId == Guid.Empty) throw new ArgumentException("NotificationId must not be an empty GUID.", nameof(notificationId));
        if (attemptNumber  < 1)           throw new ArgumentOutOfRangeException(nameof(attemptNumber), "AttemptNumber must be at least 1.");

        AttemptId      = attemptId;
        NotificationId = notificationId;
        AttemptNumber  = attemptNumber;
        StartedAt      = startedAt;
    }

    // -------------------------------------------------------------------------
    // Domain behaviour
    // -------------------------------------------------------------------------

    /// <summary>
    /// Closes the attempt by recording its outcome and the provider's raw response.
    /// This method is idempotent-safe — calling it on an already-completed attempt
    /// throws to prevent accidental overwrite of audited data.
    /// </summary>
    /// <param name="result">Outcome label (e.g., <c>"Success"</c> or <c>"Failure"</c>).</param>
    /// <param name="completedAt">UTC timestamp the call returned.</param>
    /// <param name="providerResponse">
    /// Raw provider response payload or error message (optional).
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the attempt has already been completed.
    /// </exception>
    public void Complete(string result, DateTime completedAt, string? providerResponse = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(result, nameof(result));

        if (CompletedAt.HasValue)
            throw new InvalidOperationException(
                $"DeliveryAttempt {AttemptId} has already been completed and cannot be modified.");

        if (completedAt < StartedAt)
            throw new ArgumentException(
                "CompletedAt must not be earlier than StartedAt.", nameof(completedAt));

        Result           = result;
        CompletedAt      = completedAt;
        ProviderResponse = providerResponse;
    }

    /// <summary>
    /// Convenience property: returns <see langword="true"/> when the attempt has been
    /// closed by a call to <see cref="Complete"/>.
    /// </summary>
    public bool IsCompleted => CompletedAt.HasValue;
}
