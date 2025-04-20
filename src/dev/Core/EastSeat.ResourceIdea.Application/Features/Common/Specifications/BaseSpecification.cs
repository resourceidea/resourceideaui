using System.Linq.Expressions;

using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Common.Specifications;

public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
{
    public abstract Expression<Func<TEntity, bool>> Criteria { get; }

    public BaseSpecification<TEntity> And(BaseSpecification<TEntity> specification)
    {
        return new AndSpecification<TEntity>(this, specification);
    }

    public BaseSpecification<TEntity> Or(BaseSpecification<TEntity> specification)
    {
        return new OrSpecification<TEntity>(this, specification);
    }
}
