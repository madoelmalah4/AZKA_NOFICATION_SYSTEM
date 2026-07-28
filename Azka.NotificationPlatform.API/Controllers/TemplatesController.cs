using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Azka.NotificationPlatform.API.Controllers;

/// <summary>
/// REST API controller exposing read-only template management operations.
/// Write operations (create, publish new version, deactivate) are intentionally
/// excluded from the public API surface and are performed via admin tooling or
/// migration scripts in the current iteration.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class TemplatesController : ControllerBase
{
    private readonly INotificationTemplateRepository _templateRepository;

    public TemplatesController(INotificationTemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    // ── GET /api/templates ────────────────────────────────────────────────────

    /// <summary>
    /// Returns all notification templates, optionally filtered by channel.
    /// </summary>
    /// <param name="channel">
    /// Optional channel filter: <c>0</c> = Email, <c>1</c> = SMS, <c>2</c> = Push.
    /// </param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns><c>200 OK</c> with a list of <see cref="NotificationTemplateDto"/>.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<NotificationTemplateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTemplates(
        [FromQuery] Domain.Enums.NotificationChannel? channel,
        CancellationToken cancellationToken)
    {
        var templates = await _templateRepository.GetAllAsync(channel, cancellationToken);

        var dtos = templates.Select(t => new NotificationTemplateDto
        {
            TemplateId   = t.TemplateId,
            TemplateName = t.TemplateName,
            Channel      = t.Channel,
            Subject      = t.Subject,
            Body         = t.Body,
            Language     = t.Language,
            Version      = t.Version,
            Status       = t.Status
        }).ToList();

        return Ok(dtos);
    }

    // ── GET /api/templates/{id} ───────────────────────────────────────────────

    /// <summary>
    /// Returns a single template by its surrogate key.
    /// </summary>
    /// <param name="id">The <see cref="NotificationTemplateDto.TemplateId"/>.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>
    /// <c>200 OK</c> with the <see cref="NotificationTemplateDto"/>; <c>404 Not Found</c> if absent.
    /// </returns>
    [HttpGet("{id:guid}", Name = nameof(GetTemplateById))]
    [ProducesResponseType(typeof(NotificationTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTemplateById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(id, cancellationToken);
        if (template is null) return NotFound();

        return Ok(new NotificationTemplateDto
        {
            TemplateId   = template.TemplateId,
            TemplateName = template.TemplateName,
            Channel      = template.Channel,
            Subject      = template.Subject,
            Body         = template.Body,
            Language     = template.Language,
            Version      = template.Version,
            Status       = template.Status
        });
    }
}
