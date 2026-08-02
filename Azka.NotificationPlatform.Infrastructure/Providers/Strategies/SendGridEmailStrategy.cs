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
    public async Task<(bool IsSuccess, string ProviderResponse, bool IsRecoverable)> ExecuteAsync(Notification notification, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            _logger.LogError("SendGrid ApiKey configuration is missing or empty.");
            return (false, "SendGrid ApiKey configuration is missing or empty.", false);
        }

        if (string.IsNullOrWhiteSpace(notification.Recipient) || !notification.Recipient.Contains('@'))
        {
            _logger.LogError("Invalid email recipient: {Recipient}", notification.Recipient);
            return (false, $"Invalid recipient email address '{notification.Recipient}'.", false);
        }

        try
        {
            _logger.LogInformation("Creating SendGrid client and preparing email to {Recipient} with subject '{Subject}'", notification.Recipient, notification.Subject);
            var client = new SendGridClient(_settings.ApiKey);
            var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
            var to = new EmailAddress(notification.Recipient);
            var subject = notification.Subject ?? "Notification Alert";
            
            var plainTextContent = notification.Body;
            var htmlContent = notification.Body;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            
            var response = await client.SendEmailAsync(msg, cancellationToken);
            var responseBody = await response.Body.ReadAsStringAsync();
            bool isSuccess = response.IsSuccessStatusCode;

            int statusCodeInt = (int)response.StatusCode;
            bool isRecoverable = !isSuccess && (statusCodeInt == 429 || statusCodeInt >= 500);

            var diagObject = new
            {
                statusCode = response.StatusCode,
                statusCodeInt = statusCodeInt,
                body = responseBody
            };
            string diagnosticResponse = System.Text.Json.JsonSerializer.Serialize(diagObject);
            
            if (isSuccess)
            {
                _logger.LogInformation("Successfully sent email via SendGrid. Response Code: {StatusCode}", response.StatusCode);
            }
            else
            {
                _logger.LogError("SendGrid returned non-success status code {StatusCode}. Recoverable: {IsRecoverable}. Body: {Body}", response.StatusCode, isRecoverable, responseBody);
            }

            return (isSuccess, diagnosticResponse, isRecoverable);
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Format exception for email recipient {Recipient}", notification.Recipient);
            return (false, $"{{\"error\":\"{ex.Message}\"}}", false);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Argument exception while building SendGrid email payload.");
            return (false, $"{{\"error\":\"{ex.Message}\"}}", false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending email via SendGrid.");
            return (false, $"{{\"error\":\"{ex.Message}\"}}", true);
        }
    }
}
