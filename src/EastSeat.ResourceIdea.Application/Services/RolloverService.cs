using EastSeat.ResourceIdea.Application.Common;
using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EastSeat.ResourceIdea.Application.Services;

/// <summary>
/// Application service for engagement year rollover operations
/// </summary>
public class RolloverService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<RolloverService> _logger;

    public RolloverService(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        ILogger<RolloverService> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    /// <summary>
    /// Performs a dry-run rollover to preview what would be copied
    /// </summary>
    public async Task<RolloverPreviewDto> PreviewRolloverAsync(
        Guid engagementId,
        int fromYear,
        int toYear,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Previewing rollover for engagement {EngagementId} from {FromYear} to {ToYear}",
            engagementId, fromYear, toYear);

        // Implementation will:
        // 1. Load source EngagementYear and assignments
        // 2. Filter out terminated employees
        // 3. Calculate date mappings (handle leap year)
        // 4. Return preview with counts and skip reasons

        return new RolloverPreviewDto(0, 0, new List<string>());
    }

    /// <summary>
    /// Executes the engagement year rollover
    /// </summary>
    public async Task<Result<Guid>> ExecuteRolloverAsync(
        Guid engagementId,
        int fromYear,
        int toYear,
        CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
        {
            return Result<Guid>.Failure("User must be authenticated to perform rollover.");
        }

        _logger.LogInformation("Executing rollover for engagement {EngagementId} from {FromYear} to {ToYear}",
            engagementId, fromYear, toYear);

        // Implementation will:
        // 1. Validate target year doesn't exist
        // 2. Create new EngagementYear for target year
        // 3. Copy assignments with date adjustments:
        //    - Map source day-of-year to target
        //    - Handle Feb 29 in leap year transitions (shift to Feb 28)
        //    - Exclude terminated employees
        // 4. Create RolloverHistory record
        // 5. Save in transaction

        var historyResult = RolloverHistory.Create(
            engagementId,
            fromYear,
            toYear,
            _currentUser.UserId.Value,
            0, // copied count
            0, // skipped count
            "Rollover executed"
        );

        if (!historyResult.IsSuccess)
        {
            return Result<Guid>.Failure(historyResult.Error!);
        }

        return Result<Guid>.Success(historyResult.Value!.Id);
    }
}

public record RolloverPreviewDto(
    int TotalAssignments,
    int WillBeCopied,
    List<string> SkipReasons
);
