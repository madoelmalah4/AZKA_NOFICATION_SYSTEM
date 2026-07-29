using System.Text.RegularExpressions;
using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Enums;

namespace Azka.NotificationPlatform.Application.Services;

public sealed class TemplateRendererService : ITemplateRendererService
{
    private readonly INotificationTemplateRepository _templateRepository;

    public TemplateRendererService(INotificationTemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<(string? RenderedSubject, string RenderedBody)> RenderAsync(
        string notificationType,
        NotificationChannel channel,
        string language,
        IReadOnlyDictionary<string, string> templateData,
        CancellationToken cancellationToken = default)
    {
        var template = await _templateRepository.GetActiveTemplateAsync(
            notificationType, channel, language, cancellationToken);

        if (template is null)
        {
            throw new InvalidOperationException($"No active template found for Type '{notificationType}', Channel '{channel}', Language '{language}'.");
        }

        string? renderedSubject = template.Subject;
        if (!string.IsNullOrEmpty(renderedSubject))
        {
            renderedSubject = Interpolate(renderedSubject, templateData);
        }

        string renderedBody = Interpolate(template.Body, templateData);

        return (renderedSubject, renderedBody);
    }

    private static string Interpolate(string text, IReadOnlyDictionary<string, string> data)
    {
        if (string.IsNullOrEmpty(text) || data == null || data.Count == 0)
            return text;

        return Regex.Replace(text, @"\{\{(.+?)\}\}", match =>
        {
            var key = match.Groups[1].Value.Trim();
            return data.TryGetValue(key, out var value) ? value : match.Value;
        });
    }
}
