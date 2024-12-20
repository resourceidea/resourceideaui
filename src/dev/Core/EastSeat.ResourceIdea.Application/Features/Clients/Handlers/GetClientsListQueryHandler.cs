﻿using AutoMapper;

using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class GetClientsListQueryHandler(
    IClientsService clientsService,
    IMapper mapper) : IRequestHandler<GetClientsListQuery, ResourceIdeaResponse<PagedListResponse<ClientModel>>>
{
    private readonly IClientsService _clientsService = clientsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedListResponse<ClientModel>>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
    {
        var specification = GetClientQuerySpecification(request.Filter);
        var result = await _clientsService.GetPagedListAsync(
            page: request.CurrentPageNumber,
            size: request.PageSize,
            specification,
            cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<PagedListResponse<ClientModel>>.Failure(ErrorCode.DataStoreQueryFailure);
        }

        return _mapper.Map<ResourceIdeaResponse<PagedListResponse<ClientModel>>>(result);
    }

    private static BaseSpecification<Client> GetClientQuerySpecification(string combinedFilters)
    {
        var filters = combinedFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);
        return new ClientNameSpecification(filters);
    }
}
