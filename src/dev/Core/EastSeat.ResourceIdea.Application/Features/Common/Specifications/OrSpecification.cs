using System.Linq.Expressions;

using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Common.Specifications;

internal sealed class OrSpecification<TEntity>(BaseSpecification<TEntity> left, BaseSpecification<TEntity> right)
    : BaseSpecification<TEntity> where TEntity : BaseEntity
{
    private readonly BaseSpecification<TEntity> _left = left;
    private readonly BaseSpecification<TEntity> _right = right;

    public override Expression<Func<TEntity, bool>> Criteria
    {
        get
        {
            var parameter = Expression.Parameter(typeof(TEntity));

            var leftExpression = Expression.Invoke(_left.Criteria, parameter);
            var rightExpression = Expression.Invoke(_right.Criteria, parameter);

            var orExpression = Expression.OrElse(leftExpression, rightExpression);

            return Expression.Lambda<Func<TEntity, bool>>(orExpression, parameter);
        }
    }

}