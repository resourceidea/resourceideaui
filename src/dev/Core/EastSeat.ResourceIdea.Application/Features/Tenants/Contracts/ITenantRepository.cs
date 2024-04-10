using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;

public interface ITenantRepository : IAsyncRepository<Tenant> { }