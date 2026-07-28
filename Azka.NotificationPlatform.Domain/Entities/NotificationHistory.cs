using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Domain.Entities;

/// <summary>
/// An immutable audit-log entry recording every status transition that a
/// <see cref="Notification"/> undergoes during its lifecycle (FR-7 Delivery Status).
/// </summary>
/// <remarks>
/// A <see cref="NotificationHistory"/> record is appended — never mutated — each time
/// the application layer advances the status of a <see cref="Notification"/>. Together,
/// the full set of history records for a given <see cref="NotificationId"/> forms a
/// time-ordered state-transition ledger, enabling:
/// <list type="bullet">
///   <item>Exact-time reconstruction of when each status change occurred.</item>
///   <item>Root-cause analysis using the optional <see cref="Remarks"/> field.</item>
///   <item>SLA compliance reporting and auditing.</item>
/// </list>
///
/// Relationship:
///   Notification 1 ──────────── * NotificationHistory
/// </remarks>
public sealed class NotificationHistory
{
    /// <summary>
    /// Primary surrogate key for this history entry.
    /// </summary>
    public Guid HistoryId { get; init; }

    /// <summary>
    /// Foreign key reference to the parent <see cref="Notification.NotificationId"/>
    /// whose status transition this entry records.
    /// </summary>
    public Guid NotificationId { get; init; }

    /// <summary>
    /// The <see cref="NotificationStatus"/> value that was assigned to the
    /// <see cref="Notification"/> at the time this history entry was created.
    /// Represents the destination (new) state, not the previous state.
    /// </summary>
    public NotificationStatus Status { get; init; }

    /// <summary>
    /// UTC timestamp at which the status transition occurred and this entry was created.
    /// </summary>
    public DateTime ChangedAt { get; init; }

    /// <summary>
    /// Optional free-text annotation providing human-readable context for the transition.
    /// Examples: failure reason, cancellation requester, delivery confirmation reference.
    /// </summary>
    public string? Remarks { get; init; }

    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates an immutable <see cref="NotificationHistory"/> entry. All properties
    /// are set via constructor parameters and cannot be changed after construction,
    /// preserving the integrity of the audit log.
    /// </summary>
    /// <param name="historyId">Surrogate key (platform-generated).</param>
    /// <param name="notificationId">Parent notification reference.</param>
    /// <param name="status">The new (destination) status value.</param>
    /// <param name="changedAt">UTC timestamp of the transition.</param>
    /// <param name="remarks">Optional contextual annotation.</param>
    public NotificationHistory(
        Guid historyId,
        Guid notificationId,
        NotificationStatus status,
        DateTime changedAt,
        string? remarks = null)
    {
        if (historyId      == Guid.Empty) throw new ArgumentException("HistoryId must not be an empty GUID.",      nameof(historyId));
        if (notificationId == Guid.Empty) throw new ArgumentException("NotificationId must not be an empty GUID.", nameof(notificationId));

        HistoryId      = historyId;
        NotificationId = notificationId;
        Status         = status;
        ChangedAt      = changedAt;
        Remarks        = remarks;
    }
}
