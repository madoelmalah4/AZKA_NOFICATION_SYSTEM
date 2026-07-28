using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Domain.Entities;

/// <summary>
/// Represents a reusable message template for a specific notification type, language,
/// and delivery channel (FR-3 Templates).
/// </summary>
/// <remarks>
/// Templates are versioned to allow safe rollout of content changes without affecting
/// in-flight notifications. The application layer resolves the correct template at
/// dispatch time by matching <see cref="NotificationType"/> (stored on
/// <see cref="Notification"/>), <see cref="Language"/>, and the highest active
/// <see cref="Version"/>. Template bodies support placeholder tokens (e.g.,
/// <c>{{RecipientName}}</c>) that are resolved by the application layer before the
/// rendered content is written to <see cref="Notification.Body"/>.
/// </remarks>
public sealed class NotificationTemplate
{
    /// <summary>
    /// Primary surrogate key for the template record.
    /// </summary>
    public Guid TemplateId { get; init; }

    /// <summary>
    /// A short, unique, human-readable name for the template used in administration
    /// and logging (e.g., "OrderConfirmation_EN_v2").
    /// </summary>
    public string TemplateName { get; private set; }

    /// <summary>
    /// The delivery channel this template is designed for. A notification type typically
    /// has one template per channel-language pair.
    /// </summary>
    public NotificationChannel Channel { get; private set; }

    /// <summary>
    /// The subject line of the message. Mandatory for <see cref="NotificationChannel.Email"/>;
    /// used as the push notification title; not applicable for SMS.
    /// </summary>
    public string? Subject { get; private set; }

    /// <summary>
    /// The raw body content of the template, including any placeholder tokens.
    /// Tokens follow the <c>{{TokenName}}</c> convention and are resolved at render time.
    /// </summary>
    public string Body { get; private set; }

    /// <summary>
    /// IETF BCP-47 language tag representing the locale of this template content
    /// (e.g., <c>"en-US"</c>, <c>"ar-SA"</c>, <c>"fr-FR"</c>).
    /// </summary>
    public string Language { get; private set; }

    /// <summary>
    /// Monotonically increasing integer version number for this template name.
    /// Starts at 1 and is incremented each time a new variant is published.
    /// Only the highest version with <see cref="Status"/> = Active is used for new
    /// notifications; older versions remain for audit and re-rendering purposes.
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Lifecycle status of the template. Mirrors <see cref="NotificationStatus"/> conventions
    /// but scoped to template activation: <c>"Active"</c>, <c>"Inactive"</c>, or
    /// <c>"Archived"</c>. Stored as a string to avoid coupling the template concept to
    /// the notification pipeline enum.
    /// </summary>
    public string Status { get; private set; }

    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>
    /// Initialises a new <see cref="NotificationTemplate"/> at version 1 with
    /// <c>"Active"</c> status.
    /// </summary>
    /// <param name="templateId">Surrogate key (platform-generated).</param>
    /// <param name="templateName">Unique human-readable name.</param>
    /// <param name="channel">Target delivery channel.</param>
    /// <param name="body">Raw body content with optional placeholder tokens.</param>
    /// <param name="language">IETF BCP-47 language tag.</param>
    /// <param name="version">Starting version number (must be ≥ 1).</param>
    /// <param name="subject">Optional subject / title.</param>
    public NotificationTemplate(
        Guid templateId,
        string templateName,
        NotificationChannel channel,
        string body,
        string language,
        int version = 1,
        string? subject = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName, nameof(templateName));
        ArgumentException.ThrowIfNullOrWhiteSpace(body,         nameof(body));
        ArgumentException.ThrowIfNullOrWhiteSpace(language,     nameof(language));

        if (templateId == Guid.Empty)
            throw new ArgumentException("TemplateId must not be an empty GUID.", nameof(templateId));
        if (version < 1)
            throw new ArgumentOutOfRangeException(nameof(version), "Version must be at least 1.");

        TemplateId   = templateId;
        TemplateName = templateName;
        Channel      = channel;
        Body         = body;
        Language     = language;
        Version      = version;
        Subject      = subject;
        Status       = "Active";
    }

    // -------------------------------------------------------------------------
    // Domain behaviour
    // -------------------------------------------------------------------------

    /// <summary>
    /// Publishes a new version of this template by incrementing <see cref="Version"/>,
    /// replacing content, and ensuring the template remains in Active status.
    /// </summary>
    /// <param name="newBody">Updated body content.</param>
    /// <param name="newSubject">Updated subject / title (optional).</param>
    public void PublishNewVersion(string newBody, string? newSubject = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newBody, nameof(newBody));

        Body    = newBody;
        Subject = newSubject;
        Version++;
        Status  = "Active";
    }

    /// <summary>Deactivates the template so it is no longer selected for new notifications.</summary>
    public void Deactivate()
    {
        Status = "Inactive";
    }

    /// <summary>Archives the template, marking it as read-only for historical reference.</summary>
    public void Archive()
    {
        Status = "Archived";
    }
}
