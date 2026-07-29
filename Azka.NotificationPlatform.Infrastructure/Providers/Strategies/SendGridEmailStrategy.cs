using Azka.NotificationPlatform.Application.Abstractions;
using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Azka.NotificationPlatform.Infrastructure.Providers.Strategies;

/// <summary>
/// Strategy implementation for sending emails via SendGrid (FR-2, FR-12).
/// </summary>
public sealed class SendGridEmailStrategy : INotificationProviderStrategy
{
    private readonly SendGridSettings _settings;
    private readonly Microsoft.Extensions.Logging.ILogger<SendGridEmailStrategy> _logger;

    public SendGridEmailStrategy(
        IOptions<SendGridSettings> options,
        Microsoft.Extensions.Logging.ILogger<SendGridEmailStrategy> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<(bool IsSuccess, string ProviderResponse)> ExecuteAsync(Notification notification, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            _logger.LogError("SendGrid ApiKey configuration is missing or empty.");
            return (false, "SendGrid ApiKey configuration is missing or empty.");
        }

        try
        {
            _logger.LogInformation("Creating SendGrid client and preparing email to {Recipient} with subject '{Subject}'", notification.Recipient, notification.Subject);
            var client = new SendGridClient(_settings.ApiKey);
            var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
            var to = new EmailAddress(notification.Recipient);
            var subject = notification.Subject ?? "Notification Alert";
            
            // Assume HTML content for now. A more advanced template renderer could flag HTML vs Text,
            // but for standard enterprise platforms, HTML is the baseline.
            var plainTextContent = notification.Body; // Fallback plain text
            var htmlContent = notification.Body;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            
            var response = await client.SendEmailAsync(msg, cancellationToken);
            
            var responseBody = await response.Body.ReadAsStringAsync();
            bool isSuccess = response.IsSuccessStatusCode;

            string diagnosticResponse = $"{{\"statusCode\":\"{response.StatusCode}\",\"body\":{responseBody ?? "\"\"\"\""}}}";
            
            if (isSuccess)
            {
                _logger.LogInformation("Successfully sent email via SendGrid. Response Code: {StatusCode}", response.StatusCode);
            }
            else
            {
                _logger.LogError("SendGrid returned non-success status code {StatusCode}. Body: {Body}", response.StatusCode, responseBody);
            }

            return (isSuccess, diagnosticResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending email via SendGrid.");
            return (false, $"{{\"error\":\"{ex.Message}\"}}");
        }
    }
}
