namespace Azka.NotificationPlatform.Infrastructure.Configuration;

/// <summary>
/// Strong-typed configuration settings mapping for SendGrid Email provider.
/// </summary>
public sealed class SendGridSettings
{
    public const string SectionName = "NotificationProviders:SendGrid";

    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
}
