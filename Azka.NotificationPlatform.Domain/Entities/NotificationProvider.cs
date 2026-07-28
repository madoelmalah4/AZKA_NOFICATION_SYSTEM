using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Domain.Entities;

/// <summary>
/// Represents a downstream notification service provider that the Azka platform
/// can route messages through for a specific <see cref="NotificationChannel"/>.
/// </summary>
/// <remarks>
/// Provider selection is performed at dispatch time by the application layer.
/// Multiple providers may exist for the same channel to enable failover and
/// load-distribution strategies. Only providers where <see cref="IsActive"/> is
/// <see langword="true"/> are eligible for selection.
/// Examples: "SendGrid" (Email), "Twilio" (SMS), "Firebase" (Push).
/// </remarks>
public sealed class NotificationProvider
{
    /// <summary>
    /// Primary surrogate key for the provider record.
    /// </summary>
    public Guid ProviderId { get; init; }

    /// <summary>
    /// A unique, human-readable name identifying the provider
    /// (e.g., <c>"SendGrid"</c>, <c>"Twilio"</c>, <c>"FirebaseFCM"</c>).
    /// Used in logging, alerting, and administrative UIs.
    /// </summary>
    public string ProviderName { get; private set; }

    /// <summary>
    /// The delivery channel that this provider handles. A provider is strictly
    /// mono-channel; cross-channel providers must be registered as separate records.
    /// </summary>
    public NotificationChannel Channel { get; private set; }

    /// <summary>
    /// Indicates whether the provider is available for dispatch selection.
    /// Setting this to <see langword="false"/> effectively takes the provider offline
    /// without deleting historical delivery records that reference it.
    /// </summary>
    public bool IsActive { get; private set; }

    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>
    /// Initialises a new <see cref="NotificationProvider"/> in the active state.
    /// </summary>
    /// <param name="providerId">Surrogate key (platform-generated).</param>
    /// <param name="providerName">Unique human-readable provider name.</param>
    /// <param name="channel">Delivery channel this provider handles.</param>
    /// <param name="isActive">
    /// Initial activation state. Defaults to <see langword="true"/>.
    /// </param>
    public NotificationProvider(
        Guid providerId,
        string providerName,
        NotificationChannel channel,
        bool isActive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerName, nameof(providerName));

        if (providerId == Guid.Empty)
            throw new ArgumentException("ProviderId must not be an empty GUID.", nameof(providerId));

        ProviderId   = providerId;
        ProviderName = providerName;
        Channel      = channel;
        IsActive     = isActive;
    }

    // -------------------------------------------------------------------------
    // Domain behaviour
    // -------------------------------------------------------------------------

    /// <summary>Activates the provider, making it eligible for dispatch selection.</summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Deactivates the provider, preventing it from being selected for new dispatches
    /// without removing historical delivery records.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Renames the provider. Useful when a vendor rebrand occurs and existing
    /// historical records must be preserved under the updated name.
    /// </summary>
    /// <param name="newName">The replacement provider name.</param>
    public void Rename(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName, nameof(newName));
        ProviderName = newName;
    }
}
