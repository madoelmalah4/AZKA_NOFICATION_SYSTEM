using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Application.Services;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Azka.NotificationPlatform.Tests.Application.Templates;

public class TemplateRenderingTests
{
    [Fact]
    public async Task RenderAsync_ReplacesPlaceholdersWithProvidedValues()
    {
        var templateRepoMock = new Mock<INotificationTemplateRepository>();
        var template = new NotificationTemplate(
            Guid.NewGuid(),
            "ReceiptNotification",
            NotificationChannel.Email,
            "Hello {{CustomerName}}, your payment of {{Amount}} for receipt {{ReceiptNumber}} was successful.",
            "en-US",
            1,
            "Receipt {{ReceiptNumber}} Confirmed");

        templateRepoMock
            .Setup(r => r.GetActiveTemplateAsync("ReceiptNotification", NotificationChannel.Email, "en-US", It.IsAny<CancellationToken>()))
            .ReturnsAsync(template);

        var service = new TemplateRendererService(templateRepoMock.Object);

        var data = new Dictionary<string, string>
        {
            { "CustomerName", "Jane Doe" },
            { "Amount", "$150.00" },
            { "ReceiptNumber", "REC-9988" }
        };

        var (subject, body) = await service.RenderAsync("ReceiptNotification", NotificationChannel.Email, "en-US", data);

        subject.Should().Be("Receipt REC-9988 Confirmed");
        body.Should().Be("Hello Jane Doe, your payment of $150.00 for receipt REC-9988 was successful.");
    }

    [Fact]
    public async Task RenderAsync_MissingPlaceholderKey_LeavesPlaceholderUnchanged()
    {
        var templateRepoMock = new Mock<INotificationTemplateRepository>();
        var template = new NotificationTemplate(
            Guid.NewGuid(),
            "ReceiptNotification",
            NotificationChannel.Email,
            "Hello {{CustomerName}}, your code is {{MissingCode}}.",
            "en-US",
            1,
            "Subject");

        templateRepoMock
            .Setup(r => r.GetActiveTemplateAsync("ReceiptNotification", NotificationChannel.Email, "en-US", It.IsAny<CancellationToken>()))
            .ReturnsAsync(template);

        var service = new TemplateRendererService(templateRepoMock.Object);

        var data = new Dictionary<string, string>
        {
            { "CustomerName", "Jane Doe" }
        };

        var (_, body) = await service.RenderAsync("ReceiptNotification", NotificationChannel.Email, "en-US", data);

        body.Should().Be("Hello Jane Doe, your code is {{MissingCode}}.");
    }
}
