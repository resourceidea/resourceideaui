﻿using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Responses;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Queries;

/// <summary>
/// Query object used to retrieve a single client by its ID.
/// </summary>
public class GetClientByIdQuery : IRequest<BaseResponse<ClientDTO>>
{
    /// <summary>Client ID.</summary>
    public Guid Id { get; set; }
}
