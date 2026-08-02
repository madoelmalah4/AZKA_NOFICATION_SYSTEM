using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.Features.Notifications.Commands;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Tests.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Azka.NotificationPlatform.Tests.Application.Notifications;

public class DuplicatePreventionTests
{
    [Fact]
    public async Task Handle_SameCorrelationId_ReturnsExistingNotificationWithoutReProcessing()
    {
        var correlationId = Guid.NewGuid();
        var existingNotificationId = Guid.NewGuid();
        var requestedAt = DateTime.UtcNow;

        var existingNotification = TestData.CreateNotification(
            id: existingNotificationId,
            correlationId: correlationId,
            requestedAt: requestedAt);

        var notificationRepoMock = new Mock<INotificationRepository>();
        notificationRepoMock
            .Setup(r => r.GetByCorrelationIdAsync(correlationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingNotification);

        var historyRepoMock = new Mock<INotificationHistoryRepository>();
        var templateRendererMock = new Mock<ITemplateRendererService>();
        var queueMock = new Mock<INotificationQueue>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var handler = new SendNotificationCommandHandler(
            notificationRepoMock.Object,
            historyRepoMock.Object,
            templateRendererMock.Object,
            queueMock.Object,
            unitOfWorkMock.Object);

        var command = new SendNotificationCommand
        {
            CorrelationId = correlationId,
            NotificationType = "UserRegistration",
            Recipient = "user@example.com",
            Channel = NotificationChannel.Email
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.NotificationId.Should().Be(existingNotificationId);
        result.CorrelationId.Should().Be(correlationId);

        // Verify that no duplicate entity was persisted or enqueued
        notificationRepoMock.Verify(r => r.AddAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Never);
        templateRendererMock.Verify(t => t.RenderAsync(It.IsAny<string>(), It.IsAny<NotificationChannel>(), It.IsAny<string>(), It.IsAny<IReadOnlyDictionary<string, string>>(), It.IsAny<CancellationToken>()), Times.Never);
        queueMock.Verify(q => q.EnqueueAsync(It.IsAny<Guid>()), Times.Never);
    }
}
