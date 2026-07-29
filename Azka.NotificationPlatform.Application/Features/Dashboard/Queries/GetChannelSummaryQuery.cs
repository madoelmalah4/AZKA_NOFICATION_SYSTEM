using Azka.NotificationPlatform.Application.DTOs;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Dashboard.Queries;

/// <summary>Returns per-channel notification statistics (FR-10).</summary>
public sealed record GetChannelSummaryQuery : IRequest<ChannelSummaryDto>;
