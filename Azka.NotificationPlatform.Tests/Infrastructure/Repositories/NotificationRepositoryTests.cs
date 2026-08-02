using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;
using Azka.NotificationPlatform.Tests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Azka.NotificationPlatform.Tests.Infrastructure.Repositories;

public class NotificationRepositoryTests
{
    private NotificationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new NotificationDbContext(options);
    }

    [Fact]
    public async Task AddAsync_And_GetByIdAsync_PersistsNotificationCorrectly()
    {
        using var db = CreateInMemoryDbContext();
        var repo = new NotificationRepository(db);

        var notification = TestData.CreateNotification();

        await repo.AddAsync(notification);
        await db.SaveChangesAsync();

        var retrieved = await repo.GetByIdAsync(notification.NotificationId);

        retrieved.Should().NotBeNull();
        retrieved!.NotificationId.Should().Be(notification.NotificationId);
        retrieved.Recipient.Should().Be(notification.Recipient);
    }

    [Fact]
    public async Task GetByCorrelationIdAsync_ReturnsMatchingNotification()
    {
        using var db = CreateInMemoryDbContext();
        var repo = new NotificationRepository(db);

        var correlationId = Guid.NewGuid();
        var notification = TestData.CreateNotification(correlationId: correlationId);

        await repo.AddAsync(notification);
        await db.SaveChangesAsync();

        var retrieved = await repo.GetByCorrelationIdAsync(correlationId);

        retrieved.Should().NotBeNull();
        retrieved!.CorrelationId.Should().Be(correlationId);
    }
}
