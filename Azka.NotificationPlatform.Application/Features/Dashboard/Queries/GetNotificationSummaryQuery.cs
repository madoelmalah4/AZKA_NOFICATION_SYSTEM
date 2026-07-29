using Azka.NotificationPlatform.Application.DTOs;
using MediatR;

namespace Azka.NotificationPlatform.Application.Features.Dashboard.Queries;

/// <summary>Returns the overall notifications summary (FR-10).</summary>
public sealed record GetNotificationSummaryQuery : IRequest<NotificationSummaryDto>;
