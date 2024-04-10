using System.Linq.Expressions;

using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Common.Specifications;

internal sealed class AndSpecification<TEntity>(BaseSpecification<TEntity> left, BaseSpecification<TEntity> right)
    : BaseSpecification<TEntity> where TEntity : BaseEntity
{
    private readonly BaseSpecification<TEntity> _left = left;
    private readonly BaseSpecification<TEntity> _right = right;

    public override Expression<Func<TEntity, bool>> Criteria
    {
        get
        {
            var parameter = Expression.Parameter(typeof(TEntity));

            var combined = Expression.Lambda<Func<TEntity, bool>>(
                Expression.And(
                    Expression.Invoke(_left.Criteria, parameter),
                    Expression.Invoke(_right.Criteria, parameter)
                ),
                parameter
            );

            return combined;
        }
    }
}