using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Persistence.Specifications;

/// <summary>
/// Repository query specification.
/// </summary>
/// <typeparam name="TEntity">Specification entity.</typeparam>
public abstract class Specification<TEntity>(Expression<Func<TEntity, bool>> criteria)
    where TEntity : BaseSubscriptionEntity
{
    public Expression<Func<TEntity, bool>>? Criteria { get; private set; } = criteria;
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) =>
        IncludeExpressions.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
        OrderByExpression = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression) =>
        OrderByDescendingExpression = orderByDescendingExpression;
}
