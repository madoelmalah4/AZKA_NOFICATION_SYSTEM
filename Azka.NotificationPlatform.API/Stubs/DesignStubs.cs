using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;

namespace Azka.NotificationPlatform.API.Stubs;

public class StubTemplateRendererService : ITemplateRendererService
{
    public Task<(string? RenderedSubject, string RenderedBody)> RenderAsync(
        string notificationType,
        Domain.Enums.NotificationChannel channel,
        string language,
        IReadOnlyDictionary<string, string> templateData,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<(string?, string)>((notificationType + " Subject", "Rendered body content"));
    }
}

public class StubNotificationDispatcherService : INotificationDispatcherService
{
    public Task<(string Result, string? ProviderResponse)> DispatchAsync(
        Notification notification,
        NotificationProvider provider,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<(string Result, string? ProviderResponse)>(("Success", "Delivered via stub"));
    }
}
