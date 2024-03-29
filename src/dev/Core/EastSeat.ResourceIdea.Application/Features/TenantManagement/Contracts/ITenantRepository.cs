using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Features.TenantManagement.Contracts;

public interface ITenantRepository : IAsyncRepository<Tenant> { }