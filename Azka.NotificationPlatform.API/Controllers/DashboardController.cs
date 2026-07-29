using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Application.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Azka.NotificationPlatform.API.Controllers;

/// <summary>
/// REST API controller exposing dashboard analytics and platform summaries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Provides an overall summary of all notifications (FR-10).
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(NotificationSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<NotificationSummaryDto>> GetNotificationsSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetNotificationSummaryQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Provides a summary of notifications grouped by channel (FR-10).
    /// </summary>
    [HttpGet("channels")]
    [ProducesResponseType(typeof(ChannelSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ChannelSummaryDto>> GetChannelsSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetChannelSummaryQuery(), cancellationToken);
        return Ok(result);
    }
}
