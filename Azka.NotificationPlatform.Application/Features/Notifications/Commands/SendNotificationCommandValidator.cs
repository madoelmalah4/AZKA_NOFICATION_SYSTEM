using FluentValidation;

namespace Azka.NotificationPlatform.Application.Features.Notifications.Commands;

/// <summary>
/// FluentValidation validator for <see cref="SendNotificationCommand"/>.
/// Registered automatically via <c>AddValidatorsFromAssembly</c> in DI setup.
/// </summary>
public sealed class SendNotificationCommandValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationCommandValidator()
    {
        RuleFor(x => x.CorrelationId)
            .NotEmpty()
            .WithMessage("CorrelationId (idempotency key) must be a non-empty GUID.");

        RuleFor(x => x.NotificationType)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("NotificationType must be provided and cannot exceed 100 characters.");

        RuleFor(x => x.Recipient)
            .NotEmpty()
            .MaximumLength(320)
            .WithMessage("Recipient must be provided and cannot exceed 320 characters.");

        RuleFor(x => x.Channel)
            .IsInEnum()
            .WithMessage("Channel must be a valid NotificationChannel value (Email=0, SMS=1, Push=2).");

        RuleFor(x => x.Language)
            .NotEmpty()
            .Matches(@"^[a-zA-Z]{2,3}(-[a-zA-Z]{2,4})?$")
            .WithMessage("Language must be a valid IETF BCP-47 tag (e.g., 'en-US', 'ar-SA').");

        RuleFor(x => x.RequestedAt)
            .NotEmpty()
            .LessThanOrEqualTo(_ => DateTime.UtcNow.AddMinutes(5))
            .WithMessage("RequestedAt must be a past or present UTC timestamp (max 5 minutes in the future to account for clock skew).");
    }
}
