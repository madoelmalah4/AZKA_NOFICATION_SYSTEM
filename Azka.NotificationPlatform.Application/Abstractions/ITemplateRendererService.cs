using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.Abstractions;

/// <summary>
/// Service contract for resolving and rendering a <see cref="NotificationTemplate"/>
/// against a dynamic data payload (FR-3 Template Management).
/// </summary>
/// <remarks>
/// Implementations are responsible for:
/// <list type="number">
///   <item>Looking up the correct active template from the repository.</item>
///   <item>Substituting <c>{{TokenName}}</c> placeholder tokens with values from the data dictionary.</item>
///   <item>Returning the rendered subject and body strings ready to be assigned to the notification.</item>
/// </list>
/// </remarks>
public interface ITemplateRendererService
{
    /// <summary>
    /// Resolves and renders the best matching active template for the given
    /// notification type, channel, and language.
    /// </summary>
    /// <param name="notificationType">Business type label (e.g., <c>"OrderConfirmation"</c>).</param>
    /// <param name="channel">Target delivery channel.</param>
    /// <param name="language">IETF BCP-47 language tag (e.g., <c>"en-US"</c>).</param>
    /// <param name="templateData">
    /// Key-value pairs whose keys match placeholder token names in the template body.
    /// </param>
    /// <param name="cancellationToken">Propagates cancellation from the caller.</param>
    /// <returns>
    /// A tuple of the rendered (Subject, Body). Subject may be <see langword="null"/>
    /// for SMS templates.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no active template is found for the given parameters.
    /// </exception>
    Task<(string? RenderedSubject, string RenderedBody)> RenderAsync(
        string notificationType,
        NotificationChannel channel,
        string language,
        IReadOnlyDictionary<string, string> templateData,
        CancellationToken cancellationToken = default);
}
