using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;
using Azka.NotificationPlatform.Infrastructure.Queue;
using Azka.NotificationPlatform.Tests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Azka.NotificationPlatform.Tests.Infrastructure.Retry;

public class RetryLogicTests
{
    private IServiceProvider CreateTestServiceProvider(string dbName, IProviderFactory providerFactory)
    {
        var services = new ServiceCollection();

        services.AddDbContext<NotificationDbContext>(options =>
            options.UseInMemoryDatabase(dbName));

        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationProviderRepository, NotificationProviderRepository>();
        services.AddScoped<IDeliveryAttemptRepository, DeliveryAttemptRepository>();
        services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton(providerFactory);

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task RetryLogic_RecoverableFailure_RetriesUpTo3Times()
    {
        var dbName = Guid.NewGuid().ToString();
        var notificationId = Guid.NewGuid();

        var strategyMock = new Mock<INotificationProviderStrategy>();
        strategyMock
            .Setup(s => s.ExecuteAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, "{\"statusCode\":\"ServiceUnavailable\"}", true)); // IsRecoverable = true

        var providerFactoryMock = new Mock<IProviderFactory>();
        providerFactoryMock
            .Setup(f => f.GetStrategy(NotificationChannel.Email))
            .Returns(strategyMock.Object);

        var serviceProvider = CreateTestServiceProvider(dbName, providerFactoryMock.Object);

        // Seed data using initial scope
        using (var scope = serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            var notification = TestData.CreateNotification(id: notificationId);
            notification.MarkAsQueued();
            db.Notifications.Add(notification);

            var provider = new NotificationProvider(Guid.NewGuid(), "SendGrid", NotificationChannel.Email, isActive: true);
            db.NotificationProviders.Add(provider);
            await db.SaveChangesAsync();
        }

        var queueMock = new Mock<INotificationQueue>();
        var loggerMock = new Mock<ILogger<NotificationWorker>>();

        queueMock
            .SetupSequence(q => q.DequeueAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(notificationId)
            .Throws(new OperationCanceledException());

        var worker = new NotificationWorker(queueMock.Object, serviceProvider, loggerMock.Object);

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(5000);

        await worker.StartAsync(cts.Token);
        await Task.Delay(1000);
        await worker.StopAsync(CancellationToken.None);

        // Verify 3 attempts were made
        strategyMock.Verify(s => s.ExecuteAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Exactly(3));

        using (var scope = serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            var attempts = await db.DeliveryAttempts.Where(a => a.NotificationId == notificationId).ToListAsync();
            attempts.Should().HaveCount(3);

            var updatedNotification = await db.Notifications.FindAsync(notificationId);
            updatedNotification!.Status.Should().Be(NotificationStatus.Failed);
        }
    }

    [Fact]
    public async Task RetryLogic_NonRecoverableFailure_FailsImmediatelyWithoutRetries()
    {
        var dbName = Guid.NewGuid().ToString();
        var notificationId = Guid.NewGuid();

        var strategyMock = new Mock<INotificationProviderStrategy>();
        strategyMock
            .Setup(s => s.ExecuteAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, "{\"statusCode\":\"BadRequest\",\"error\":\"Invalid recipient\"}", false)); // IsRecoverable = false

        var providerFactoryMock = new Mock<IProviderFactory>();
        providerFactoryMock
            .Setup(f => f.GetStrategy(NotificationChannel.Email))
            .Returns(strategyMock.Object);

        var serviceProvider = CreateTestServiceProvider(dbName, providerFactoryMock.Object);

        // Seed data using initial scope
        using (var scope = serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            var notification = TestData.CreateNotification(id: notificationId, recipient: "invalid-email");
            notification.MarkAsQueued();
            db.Notifications.Add(notification);

            var provider = new NotificationProvider(Guid.NewGuid(), "SendGrid", NotificationChannel.Email, isActive: true);
            db.NotificationProviders.Add(provider);
            await db.SaveChangesAsync();
        }

        var queueMock = new Mock<INotificationQueue>();
        var loggerMock = new Mock<ILogger<NotificationWorker>>();

        queueMock
            .SetupSequence(q => q.DequeueAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(notificationId)
            .Throws(new OperationCanceledException());

        var worker = new NotificationWorker(queueMock.Object, serviceProvider, loggerMock.Object);

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(5000);

        await worker.StartAsync(cts.Token);
        await Task.Delay(1000);
        await worker.StopAsync(CancellationToken.None);

        // Verify only 1 attempt was made (0 additional retries)
        strategyMock.Verify(s => s.ExecuteAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Once);

        using (var scope = serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            var attempts = await db.DeliveryAttempts.Where(a => a.NotificationId == notificationId).ToListAsync();
            attempts.Should().HaveCount(1);

            var updatedNotification = await db.Notifications.FindAsync(notificationId);
            updatedNotification!.Status.Should().Be(NotificationStatus.Failed);
        }
    }
}
