using Azka.NotificationPlatform.Domain.Enums;
using Azka.NotificationPlatform.Infrastructure.Configuration;
using Azka.NotificationPlatform.Infrastructure.Providers;
using Azka.NotificationPlatform.Infrastructure.Providers.Strategies;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Azka.NotificationPlatform.Tests.Infrastructure.Providers;

public class ProviderFactoryTests
{
    [Fact]
    public void GetStrategy_ReturnsCorrectStrategyForEachChannel()
    {
        var services = new ServiceCollection();

        var sendGridLogger = new Mock<ILogger<SendGridEmailStrategy>>().Object;
        var sendGridOptions = Options.Create(new SendGridSettings());

        var twilioLogger = new Mock<ILogger<TwilioSmsStrategy>>().Object;
        var firebaseLogger = new Mock<ILogger<FirebasePushStrategy>>().Object;
        var firebaseOptions = Options.Create(new FirebaseSettings());

        services.AddTransient(_ => new SendGridEmailStrategy(sendGridOptions, sendGridLogger));
        services.AddTransient(_ => new TwilioSmsStrategy());
        services.AddTransient(_ => new FirebasePushStrategy(firebaseOptions, firebaseLogger));

        var serviceProvider = services.BuildServiceProvider();
        var factory = new ProviderFactory(serviceProvider);

        var emailStrategy = factory.GetStrategy(NotificationChannel.Email);
        emailStrategy.Should().BeOfType<SendGridEmailStrategy>();

        var smsStrategy = factory.GetStrategy(NotificationChannel.SMS);
        smsStrategy.Should().BeOfType<TwilioSmsStrategy>();

        var pushStrategy = factory.GetStrategy(NotificationChannel.Push);
        pushStrategy.Should().BeOfType<FirebasePushStrategy>();
    }
}
