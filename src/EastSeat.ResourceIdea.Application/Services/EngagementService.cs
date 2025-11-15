using EastSeat.ResourceIdea.Application.Common;
using EastSeat.ResourceIdea.Application.DTOs;
using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace EastSeat.ResourceIdea.Application.Services;

/// <summary>
/// Application service for managing engagements
/// </summary>
public class EngagementService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<EngagementService> _logger;

    public EngagementService(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        ILogger<EngagementService> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<Result<Guid>> CreateEngagementAsync(CreateEngagementDto dto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating engagement: {Code} for client {ClientId}",
            dto.Code, dto.ClientId);

        var engagementResult = Engagement.Create(
            dto.ClientId,
            dto.Code,
            dto.Title,
            dto.Type,
            dto.StartDate,
            dto.EndDate,
            dto.PartnerId,
            dto.ManagerId,
            dto.BudgetHours
        );

        if (!engagementResult.IsSuccess)
        {
            _logger.LogWarning("Failed to create engagement: {Error}", engagementResult.Error);
            return Result<Guid>.Failure(engagementResult.Error!);
        }

        // Create initial engagement year for start year
        var year = dto.StartDate.Year;
        var engagementYearResult = EngagementYear.Create(engagementResult.Value!.Id, year);

        if (!engagementYearResult.IsSuccess)
        {
            return Result<Guid>.Failure(engagementYearResult.Error!);
        }

        _logger.LogInformation("Engagement created successfully: {Id}", engagementResult.Value!.Id);
        return Result<Guid>.Success(engagementResult.Value!.Id);
    }

    // Additional methods: GetEngagement, ListEngagements, UpdateEngagement, 
    // AssignPartner, AssignManager, CompleteEngagement
}
