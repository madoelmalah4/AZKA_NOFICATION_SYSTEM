using Azka.NotificationPlatform.API.Controllers;
using Azka.NotificationPlatform.API.Models;
using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Application.Features.Notifications.Commands;
using Azka.NotificationPlatform.Application.Features.Notifications.Queries;
using Azka.NotificationPlatform.Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Azka.NotificationPlatform.Tests.API.Notifications;

public class NotificationsControllerTests
{
    [Fact]
    public async Task SendNotification_ValidRequest_ReturnsAcceptedAtAction()
    {
        var mediatorMock = new Mock<IMediator>();
        var notificationId = Guid.NewGuid();

        var dto = new NotificationDto
        {
            NotificationId = notificationId,
            NotificationType = "UserRegistration",
            Recipient = "user@example.com",
            Channel = NotificationChannel.Email,
            Body = "Welcome!",
            Status = NotificationStatus.Pending,
            CorrelationId = Guid.NewGuid(),
            RequestedAt = DateTime.UtcNow
        };

        mediatorMock
            .Setup(m => m.Send(It.IsAny<SendNotificationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new NotificationsController(mediatorMock.Object);
        var request = new SendNotificationRequest
        {
            NotificationType = "UserRegistration",
            Recipient = "user@example.com",
            Channel = NotificationChannel.Email
        };

        var result = await controller.SendNotification(request, CancellationToken.None);

        var acceptedResult = result.Should().BeOfType<AcceptedAtActionResult>().Subject;
        acceptedResult.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetNotificationById_ExistingId_ReturnsOkResult()
    {
        var mediatorMock = new Mock<IMediator>();
        var notificationId = Guid.NewGuid();

        var dto = new NotificationDto
        {
            NotificationId = notificationId,
            NotificationType = "UserRegistration",
            Recipient = "user@example.com",
            Channel = NotificationChannel.Email,
            Body = "Welcome!",
            Status = NotificationStatus.Delivered,
            CorrelationId = Guid.NewGuid(),
            RequestedAt = DateTime.UtcNow
        };

        mediatorMock
            .Setup(m => m.Send(It.Is<GetNotificationByIdQuery>(q => q.NotificationId == notificationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new NotificationsController(mediatorMock.Object);

        var result = await controller.GetNotificationById(notificationId, CancellationToken.None);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetNotificationById_NonExistingId_ReturnsNotFoundResult()
    {
        var mediatorMock = new Mock<IMediator>();
        var notificationId = Guid.NewGuid();

        mediatorMock
            .Setup(m => m.Send(It.IsAny<GetNotificationByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((NotificationDto?)null);

        var controller = new NotificationsController(mediatorMock.Object);

        var result = await controller.GetNotificationById(notificationId, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task SearchNotifications_ValidQuery_ReturnsOkPagedResult()
    {
        var mediatorMock = new Mock<IMediator>();
        var pagedResult = new PagedResult<NotificationDto>
        {
            Items = new List<NotificationDto>(),
            PageNumber = 1,
            PageSize = 20,
            TotalCount = 0,
            TotalPages = 0
        };

        mediatorMock
            .Setup(m => m.Send(It.IsAny<SearchNotificationsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        var controller = new NotificationsController(mediatorMock.Object);

        var result = await controller.SearchNotifications(new SearchNotificationsQuery(), CancellationToken.None);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(pagedResult);
    }
}
