using Azka.NotificationPlatform.Application.Features.Notifications.Queries;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Infrastructure.Persistence;
using Azka.NotificationPlatform.Infrastructure.Persistence.Repositories;
using Azka.NotificationPlatform.Tests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Azka.NotificationPlatform.Tests.Application.Notifications;

public class SearchNotificationsTests
{
    private NotificationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new NotificationDbContext(options);
    }

    private async Task SeedDataAsync(NotificationDbContext db)
    {
        var baseDate = new DateTime(2026, 8, 1, 12, 0, 0, DateTimeKind.Utc);

        var n1 = TestData.CreateNotification(
            id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            type: "UserRegistration",
            recipient: "user1@example.com",
            channel: NotificationChannel.Email,
            correlationId: Guid.Parse("c1111111-1111-1111-1111-111111111111"),
            requestedAt: baseDate,
            appName: "AuthService");
        n1.MarkAsQueued();
        n1.MarkAsProcessing();
        n1.MarkAsDelivered();

        var n2 = TestData.CreateNotification(
            id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            type: "OrderConfirmation",
            recipient: "user2@example.com",
            channel: NotificationChannel.SMS,
            correlationId: Guid.Parse("c2222222-2222-2222-2222-222222222222"),
            requestedAt: baseDate.AddHours(2),
            appName: "PaymentSystem");
        n2.MarkAsQueued();
        n2.MarkAsProcessing();
        n2.MarkAsFailed();

        var n3 = TestData.CreateNotification(
            id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            type: "PasswordReset",
            recipient: "user1@example.com",
            channel: NotificationChannel.Email,
            correlationId: Guid.Parse("c3333333-3333-3333-3333-333333333333"),
            requestedAt: baseDate.AddDays(1),
            appName: "AuthService");

        db.Notifications.AddRange(n1, n2, n3);
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task Search_NoFilters_ReturnsAllNotifications()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var result = await repo.SearchAsync(new SearchNotificationsQuery());

        result.TotalCount.Should().Be(3);
        result.Items.Should().HaveCount(3);
    }

    [Fact]
    public async Task Search_FilterByNotificationId_ReturnsMatchingItem()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);
        var targetId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var result = await repo.SearchAsync(new SearchNotificationsQuery { NotificationId = targetId });

        result.TotalCount.Should().Be(1);
        result.Items[0].NotificationId.Should().Be(targetId);
    }

    [Fact]
    public async Task Search_FilterByRecipient_ReturnsMatchingRecipient()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var result = await repo.SearchAsync(new SearchNotificationsQuery { Recipient = "user1@example.com" });

        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(x => x.Recipient == "user1@example.com");
    }

    [Fact]
    public async Task Search_FilterByChannel_ReturnsMatchingChannel()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var result = await repo.SearchAsync(new SearchNotificationsQuery { Channel = NotificationChannel.SMS });

        result.TotalCount.Should().Be(1);
        result.Items[0].Channel.Should().Be(NotificationChannel.SMS);
    }

    [Fact]
    public async Task Search_FilterByStatus_ReturnsMatchingStatus()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var result = await repo.SearchAsync(new SearchNotificationsQuery { Status = NotificationStatus.Delivered });

        result.TotalCount.Should().Be(1);
        result.Items[0].Status.Should().Be(NotificationStatus.Delivered);
    }

    [Fact]
    public async Task Search_FilterByNotificationType_ReturnsMatchingType()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var result = await repo.SearchAsync(new SearchNotificationsQuery { NotificationType = "UserRegistration" });

        result.TotalCount.Should().Be(1);
        result.Items[0].NotificationType.Should().Be("UserRegistration");
    }

    [Fact]
    public async Task Search_FilterByCorrelationId_ReturnsMatchingCorrelationId()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);
        var targetCorrelationId = Guid.Parse("c2222222-2222-2222-2222-222222222222");

        var result = await repo.SearchAsync(new SearchNotificationsQuery { CorrelationId = targetCorrelationId });

        result.TotalCount.Should().Be(1);
        result.Items[0].CorrelationId.Should().Be(targetCorrelationId);
    }

    [Fact]
    public async Task Search_FilterByApplicationName_ReturnsMatchingApplicationName()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var result = await repo.SearchAsync(new SearchNotificationsQuery { ApplicationName = "PaymentSystem" });

        result.TotalCount.Should().Be(1);
        result.Items[0].ApplicationName.Should().Be("PaymentSystem");
    }

    [Fact]
    public async Task Search_FilterByDateRange_ReturnsNotificationsWithinRange()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var fromDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc);
        var toDate = new DateTime(2026, 8, 1, 23, 59, 59, DateTimeKind.Utc);

        var result = await repo.SearchAsync(new SearchNotificationsQuery { FromDate = fromDate, ToDate = toDate });

        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Search_CombineMultipleFilters_ReturnsExactMatches()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var result = await repo.SearchAsync(new SearchNotificationsQuery
        {
            Channel = NotificationChannel.Email,
            Status = NotificationStatus.Delivered,
            ApplicationName = "AuthService"
        });

        result.TotalCount.Should().Be(1);
        result.Items[0].NotificationId.Should().Be(Guid.Parse("11111111-1111-1111-1111-111111111111"));
    }

    [Fact]
    public async Task Search_PaginationWorksCorrectly()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var resultPage1 = await repo.SearchAsync(new SearchNotificationsQuery { PageNumber = 1, PageSize = 2 });
        resultPage1.TotalCount.Should().Be(3);
        resultPage1.TotalPages.Should().Be(2);
        resultPage1.Items.Should().HaveCount(2);

        var resultPage2 = await repo.SearchAsync(new SearchNotificationsQuery { PageNumber = 2, PageSize = 2 });
        resultPage2.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task Search_EmptyResult_ReturnsEmptyCollectionWithMetadata()
    {
        using var db = CreateInMemoryDbContext();
        await SeedDataAsync(db);
        var repo = new NotificationRepository(db);

        var result = await repo.SearchAsync(new SearchNotificationsQuery { Recipient = "nonexistent@example.com", PageNumber = 1, PageSize = 20 });

        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(20);
        result.Items.Should().BeEmpty();
    }
}
