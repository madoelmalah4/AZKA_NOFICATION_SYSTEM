using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Infrastructure.Configuration;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Azka.NotificationPlatform.Infrastructure;

/// <summary>
/// Extension method that registers all Infrastructure Layer services into the
/// dependency-injection container. Called from the API Layer's <c>Program.cs</c>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the EF Core <see cref="NotificationDbContext"/> (SQL Server),
    /// all repository implementations, and the <see cref="IUnitOfWork"/>.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="configuration">
    /// The application configuration — must contain a connection string named
    /// <c>"AzkaNotificationDb"</c>.
    /// </param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── EF Core ───────────────────────────────────────────────────────────
        services.AddDbContext<NotificationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AzkaNotificationDb"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount:       3,
                        maxRetryDelay:       TimeSpan.FromSeconds(10),
                        errorNumbersToAdd:   null);
                    sqlOptions.CommandTimeout(30);
                }));

        // ── Unit of Work ──────────────────────────────────────────────────────
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ── Repositories ──────────────────────────────────────────────────────
        services.AddScoped<INotificationRepository,         NotificationRepository>();
        services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();
        services.AddScoped<INotificationProviderRepository, NotificationProviderRepository>();
        services.AddScoped<IDeliveryAttemptRepository,      DeliveryAttemptRepository>();
        services.AddScoped<INotificationHistoryRepository,  NotificationHistoryRepository>();

        // ── High-Throughput In-Process Queue (FR-4) ───────────────────────────
        services.AddSingleton<INotificationQueue, Queue.InMemoryNotificationQueue>();
        services.AddHostedService<Queue.NotificationWorker>();

        // ── Strategies & Factory (FR-12) ──────────────────────────────────────
        services.AddTransient<Providers.Strategies.SendGridEmailStrategy>();
        services.AddTransient<Providers.Strategies.TwilioSmsStrategy>();
        services.AddTransient<Providers.Strategies.FirebasePushStrategy>();
        services.AddSingleton<IProviderFactory, Providers.ProviderFactory>();

        // ── Configuration Options Mapping ─────────────────────────────────────
        services.Configure<SendGridSettings>(configuration.GetSection(SendGridSettings.SectionName));
        services.Configure<TwilioSettings>(configuration.GetSection(TwilioSettings.SectionName));
        services.Configure<FirebaseSettings>(configuration.GetSection(FirebaseSettings.SectionName));

        return services;
    }
}
