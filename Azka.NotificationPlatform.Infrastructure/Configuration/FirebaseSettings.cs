namespace Azka.NotificationPlatform.Infrastructure.Configuration;

/// <summary>
/// Strong-typed configuration settings mapping for Firebase Push provider.
/// </summary>
public sealed class FirebaseSettings
{
    public const string SectionName = "NotificationProviders:Firebase";

    public string ProjectId { get; set; } = string.Empty;
    public string CredentialJson { get; set; } = string.Empty;
}
