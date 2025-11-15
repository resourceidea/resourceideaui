using EastSeat.ResourceIdea.Application.Common;
using EastSeat.ResourceIdea.Application.DTOs;
using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EastSeat.ResourceIdea.Application.Services;

/// <summary>
/// Application service for managing clients
/// </summary>
public class ClientService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<ClientService> _logger;

    public ClientService(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        ILogger<ClientService> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<Result<Guid>> CreateClientAsync(CreateClientDto dto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating client: {Name}", dto.Name);

        var clientResult = Client.Create(
            dto.Name,
            dto.RegistrationNumber,
            dto.Sector,
            dto.ContactEmail,
            dto.ContactPhone
        );

        if (!clientResult.IsSuccess)
        {
            _logger.LogWarning("Failed to create client: {Error}", clientResult.Error);
            return Result<Guid>.Failure(clientResult.Error!);
        }

        // In real implementation, add to DbContext and save
        // await _context.Clients.AddAsync(clientResult.Value!, cancellationToken);
        // await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Client created successfully: {Id}", clientResult.Value!.Id);
        return Result<Guid>.Success(clientResult.Value!.Id);
    }

    // Additional methods: GetClientById, ListClients, UpdateClient, DeactivateClient
}
