using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Azka.NotificationPlatform.Tests.Domain;

public class NotificationStatusTests
{
    [Fact]
    public void StatusTransitions_ValidLifecycle_Succeeds()
    {
        var notification = TestData.CreateNotification();
        notification.Status.Should().Be(NotificationStatus.Pending);

        notification.MarkAsQueued();
        notification.Status.Should().Be(NotificationStatus.Queued);

        notification.MarkAsProcessing();
        notification.Status.Should().Be(NotificationStatus.Processing);

        notification.MarkAsDelivered();
        notification.Status.Should().Be(NotificationStatus.Delivered);
    }

    [Fact]
    public void StatusTransitions_ProcessingToFailed_Succeeds()
    {
        var notification = TestData.CreateNotification();
        notification.MarkAsQueued();
        notification.MarkAsProcessing();

        notification.MarkAsFailed();
        notification.Status.Should().Be(NotificationStatus.Failed);
    }

    [Fact]
    public void StatusTransitions_InvalidTransition_ThrowsInvalidOperationException()
    {
        var notification = TestData.CreateNotification();

        // Pending directly to Delivered is invalid
        var act = () => notification.MarkAsDelivered();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void StatusTransitions_ProcessingToQueued_ThrowsInvalidOperationException()
    {
        var notification = TestData.CreateNotification();
        notification.MarkAsQueued();
        notification.MarkAsProcessing();

        var act = () => notification.MarkAsQueued();

        act.Should().Throw<InvalidOperationException>();
    }
}
