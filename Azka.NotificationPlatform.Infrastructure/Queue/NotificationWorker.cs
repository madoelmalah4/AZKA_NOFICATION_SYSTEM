using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Azka.NotificationPlatform.Infrastructure.Queue;

/// <summary>
/// Unblocked asynchronous background service that continuously processes enqueued notifications (FR-4, FR-6).
/// </summary>
public sealed class NotificationWorker : BackgroundService
{
    private readonly INotificationQueue _queue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationWorker> _logger;

    public NotificationWorker(
        INotificationQueue queue,
        IServiceProvider serviceProvider,
        ILogger<NotificationWorker> logger)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Azka Notification Platform Worker Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Dequeue a notification tracking ID (blocks until available)
                var notificationId = await _queue.DequeueAsync(stoppingToken);

                // Create scope to resolve scoped EF repositories and unit of work
                using var scope = _serviceProvider.CreateScope();
                var notificationRepo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                var providerRepo = scope.ServiceProvider.GetRequiredService<INotificationProviderRepository>();
                var attemptRepo = scope.ServiceProvider.GetRequiredService<IDeliveryAttemptRepository>();
                var historyRepo = scope.ServiceProvider.GetRequiredService<INotificationHistoryRepository>();
                var providerFactory = scope.ServiceProvider.GetRequiredService<IProviderFactory>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                // 1. Fetch Notification
                var notification = await notificationRepo.GetByIdAsync(notificationId, stoppingToken);
                if (notification is null)
                {
                    _logger.LogWarning("Notification {NotificationId} not found in database.", notificationId);
                    continue;
                }

                // 2. Mark as Processing in DB
                notification.MarkAsProcessing();
                notificationRepo.Update(notification);
                await historyRepo.AddAsync(
                    new NotificationHistory(
                        Guid.NewGuid(),
                        notification.NotificationId,
                        NotificationStatus.Processing,
                        DateTime.UtcNow,
                        "Processing initiated by worker."),
                    stoppingToken);
                await unitOfWork.SaveChangesAsync(stoppingToken);

                // 3. Resolve active provider and strategy
                var providers = await providerRepo.GetActiveByChannelAsync(notification.Channel, stoppingToken);
                if (providers.Count == 0)
                {
                    notification.MarkAsFailed();
                    notificationRepo.Update(notification);
                    await historyRepo.AddAsync(
                        new NotificationHistory(
                            Guid.NewGuid(),
                            notification.NotificationId,
                            NotificationStatus.Failed,
                            DateTime.UtcNow,
                            $"Delivery failed: No active providers registered for channel {notification.Channel}."),
                        stoppingToken);
                    await unitOfWork.SaveChangesAsync(stoppingToken);
                    continue;
                }

                var activeProvider = providers[0];
                var strategy = providerFactory.GetStrategy(notification.Channel);

                // 4. FR-6: Execution loop with 3x Transient Retries
                bool deliverySuccess = false;
                string providerResponse = string.Empty;
                const int MaxAttempts = 3;

                for (int attemptNum = 1; attemptNum <= MaxAttempts; attemptNum++)
                {
                    var startedAt = DateTime.UtcNow;
                    var attempt = new DeliveryAttempt(Guid.NewGuid(), notification.NotificationId, attemptNum, startedAt);
                    await attemptRepo.AddAsync(attempt, stoppingToken);
                    await unitOfWork.SaveChangesAsync(stoppingToken);

                    try
                    {
                        var (success, response) = await strategy.ExecuteAsync(notification, stoppingToken);
                        deliverySuccess = success;
                        providerResponse = response;

                        attempt.Complete(success ? "Success" : "Failure", DateTime.UtcNow, response);
                        attemptRepo.Update(attempt);
                        await unitOfWork.SaveChangesAsync(stoppingToken);

                        if (success)
                        {
                            break; // Delivered successfully
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred during delivery attempt {AttemptNum} for Notification {NotificationId}.", attemptNum, notificationId);
                        providerResponse = ex.Message;
                        attempt.Complete("Failure", DateTime.UtcNow, ex.Message);
                        attemptRepo.Update(attempt);
                        await unitOfWork.SaveChangesAsync(stoppingToken);
                    }
                }

                // 5. Commit final Status state to database
                if (deliverySuccess)
                {
                    notification.MarkAsDelivered();
                }
                else
                {
                    notification.MarkAsFailed();
                }

                notificationRepo.Update(notification);
                await historyRepo.AddAsync(
                    new NotificationHistory(
                        Guid.NewGuid(),
                        notification.NotificationId,
                        notification.Status,
                        DateTime.UtcNow,
                        providerResponse),
                    stoppingToken);
                await unitOfWork.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled error occurred in the background notification worker loop.");
            }
        }

        _logger.LogInformation("Azka Notification Platform Worker Service stopping.");
    }
}
