namespace Azka.NotificationPlatform.Infrastructure.Configuration;

/// <summary>
/// Strong-typed configuration settings mapping for Twilio SMS provider.
/// </summary>
public sealed class TwilioSettings
{
    public const string SectionName = "NotificationProviders:Twilio";

    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string FromPhoneNumber { get; set; } = string.Empty;
}
