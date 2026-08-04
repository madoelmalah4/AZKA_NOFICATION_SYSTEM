using Azka.NotificationPlatform.Application.DTOs;
using Azka.NotificationPlatform.Application.Features.Dashboard.Queries;
using Azka.NotificationPlatform.Application.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Azka.NotificationPlatform.API.Controllers;

public class GetProviderSummaryQuery : MediatR.IRequest<ProviderSummaryDto> { }

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

    /// <summary>
    /// Provides delivery statistics grouped by registered notification provider (FR-10).
    /// Each entry shows the provider's name, channel, activation state, and aggregated
    /// delivery counts (total, delivered, failed, attempts) with a derived success rate.
    /// </summary>
    [HttpGet("providers")]
    [ProducesResponseType(typeof(ProviderSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProviderSummaryDto>> GetProvidersSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProviderSummaryQuery(), cancellationToken);
        return Ok(result);
    }
}
