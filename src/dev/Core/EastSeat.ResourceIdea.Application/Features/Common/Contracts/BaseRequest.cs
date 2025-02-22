using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Common.Contracts;

/// <summary>
/// Base request class for all MediatR requests.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseRequest<T> : IRequest<ResourceIdeaResponse<T>> where T : class
{
    public TenantId TenantId { get; set; }
}