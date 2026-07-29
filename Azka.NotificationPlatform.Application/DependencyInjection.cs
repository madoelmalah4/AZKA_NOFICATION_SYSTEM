using Azka.NotificationPlatform.Application.Features.Notifications.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Azka.NotificationPlatform.Application;

/// <summary>
/// Extension method that registers all Application Layer services into the
/// dependency-injection container. Called from the API Layer's <c>Program.cs</c>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MediatR handlers, FluentValidation validators, and the
    /// MediatR validation pipeline behaviour for the Application Layer.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register all MediatR request handlers discovered in this assembly.
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddScoped<Azka.NotificationPlatform.Application.Abstractions.ITemplateRendererService, Azka.NotificationPlatform.Application.Services.TemplateRendererService>();

        // Register all FluentValidation validators discovered in this assembly.
        services.AddValidatorsFromAssembly(typeof(SendNotificationCommandValidator).Assembly);

        return services;
    }
}
