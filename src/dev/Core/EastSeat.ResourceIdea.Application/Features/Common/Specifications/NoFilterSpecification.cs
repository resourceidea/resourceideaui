using System.Linq.Expressions;

using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Common.Specifications;

public sealed class NoFilterSpecification<TEntity> : BaseSpecification<TEntity> where TEntity : BaseEntity
{
    public override Expression<Func<TEntity, bool>> Criteria => entity => true;
}