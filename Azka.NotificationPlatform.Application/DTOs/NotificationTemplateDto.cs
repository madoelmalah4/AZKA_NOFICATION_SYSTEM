using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.DTOs;

/// <summary>
/// Read-model / data-transfer object for a <see cref="Domain.Entities.NotificationTemplate"/>.
/// </summary>
public sealed record NotificationTemplateDto
{
    /// <summary>Template surrogate key.</summary>
    public required Guid TemplateId { get; init; }

    /// <summary>Human-readable unique template name.</summary>
    public required string TemplateName { get; init; }

    /// <summary>Delivery channel the template targets.</summary>
    public required NotificationChannel Channel { get; init; }

    /// <summary>Subject line (may be <see langword="null"/> for SMS templates).</summary>
    public string? Subject { get; init; }

    /// <summary>Raw body with placeholder tokens.</summary>
    public required string Body { get; init; }

    /// <summary>IETF BCP-47 language tag (e.g., <c>"en-US"</c>).</summary>
    public required string Language { get; init; }

    /// <summary>Current version number (≥ 1).</summary>
    public required int Version { get; init; }

    /// <summary>Lifecycle status string: <c>"Active"</c>, <c>"Inactive"</c>, or <c>"Archived"</c>.</summary>
    public required string Status { get; init; }
}
