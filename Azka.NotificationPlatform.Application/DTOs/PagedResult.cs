namespace Azka.NotificationPlatform.Application.DTOs;

/// <summary>
/// Reusable container for paginated query results (FR-9).
/// </summary>
/// <typeparam name="T">Result element DTO type.</typeparam>
public sealed record PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required int TotalCount { get; init; }
    public required int TotalPages { get; init; }
}
