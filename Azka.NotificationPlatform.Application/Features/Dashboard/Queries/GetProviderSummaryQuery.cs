using Azka.NotificationPlatform.Application.DTOs;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Dashboard.Queries;

/// <summary>
/// CQRS query: returns notification delivery statistics grouped by provider (FR-10).
/// </summary>
public sealed record GetProviderSummaryQuery : IRequest<ProviderSummaryDto>;
