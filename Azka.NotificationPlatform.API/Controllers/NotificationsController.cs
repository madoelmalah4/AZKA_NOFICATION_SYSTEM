using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Application.Features.Notifications.Commands;
using Azka.NotificationPlatform.Application.Features.Notifications.Queries;
using Azka.NotificationPlatform.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Azka.NotificationPlatform.API.Controllers;

/// <summary>
/// REST API controller exposing notification lifecycle operations.
/// All business logic is delegated to the Application Layer via MediatR.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ── POST /api/notifications ───────────────────────────────────────────────

    /// <summary>
    /// Submits a new notification request to the platform for asynchronous processing.
    /// </summary>
    /// <remarks>
    /// Idempotent: if a notification with the same <c>CorrelationId</c> already exists,
    /// the existing record is returned without creating a duplicate (FR-11).
    /// </remarks>
    /// <param name="command">The notification submission command.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>
    /// <c>202 Accepted</c> with the created/existing <see cref="NotificationDto"/>;
    /// <c>400 Bad Request</c> if the command fails validation.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendNotification(
        [FromBody] SendNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SendNotificationCommand
        {
            NotificationType = request.NotificationType,
            Recipient        = request.Recipient,
            Channel          = request.Channel
        };
        var result = await _mediator.Send(command, cancellationToken);
        return AcceptedAtAction(
            nameof(GetNotificationById),
            new { id = result.NotificationId },
            result);
    }

    // ── GET /api/notifications/{id} ───────────────────────────────────────────

    /// <summary>
    /// Retrieves a notification by its platform-assigned ID.
    /// </summary>
    /// <param name="id">The <see cref="NotificationDto.NotificationId"/>.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>
    /// <c>200 OK</c> with the <see cref="NotificationDto"/>; <c>404 Not Found</c> if not found.
    /// </returns>
    [HttpGet("{id:guid}", Name = nameof(GetNotificationById))]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotificationById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetNotificationByIdQuery { NotificationId = id },
            cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    // ── GET /api/notifications/{id}/history ───────────────────────────────────

    /// <summary>
    /// Returns the complete status-transition audit trail for a notification (FR-7).
    /// </summary>
    /// <param name="id">The parent notification ID.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns><c>200 OK</c> with an ordered list of <see cref="NotificationHistoryDto"/>.</returns>
    [HttpGet("{id:guid}/history")]
    [ProducesResponseType(typeof(IReadOnlyList<NotificationHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationHistory(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetNotificationHistoryQuery { NotificationId = id },
            cancellationToken);

        return Ok(result);
    }

    // ── DELETE /api/notifications/{id} ───────────────────────────────────────

    /// <summary>
    /// Cancels an in-flight notification that has not yet reached a terminal state.
    /// </summary>
    /// <param name="id">The notification to cancel.</param>
    /// <param name="reason">Optional human-readable cancellation reason.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>
    /// <c>204 No Content</c> on success; <c>404 Not Found</c> if not found or
    /// already in a terminal state.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelNotification(
        [FromRoute] Guid id,
        [FromQuery] string? reason,
        CancellationToken cancellationToken)
    {
        var cancelled = await _mediator.Send(
            new CancelNotificationCommand { NotificationId = id, Reason = reason },
            cancellationToken);

        return cancelled ? NoContent() : NotFound();
    }
}
