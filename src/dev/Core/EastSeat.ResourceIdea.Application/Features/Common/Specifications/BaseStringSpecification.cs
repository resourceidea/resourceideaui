﻿using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Types;

using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Common.Specifications;

/// <summary>
/// Base specification for string based specifications.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseStringSpecification<TEntity>(Dictionary<string, string> filters)
    : BaseSpecification<TEntity> where TEntity : BaseEntity
{
    protected readonly Dictionary<string, string> _filters = filters;

    public override Expression<Func<TEntity, bool>> Criteria
    {
        get
        {
            string? optionalFilter = GetValidatedStringFilter();
            string filter = optionalFilter ?? string.Empty;

            return string.IsNullOrEmpty(filter)
                ? entity => false
                : GetExpression();
        }
    }

    protected abstract Expression<Func<TEntity, bool>> GetExpression();

    private string? GetValidatedStringFilter()
    {
        if (_filters is null
            || _filters.Count <= 0
            || !_filters.TryGetValue(GetFilterKey(), out var nameValue)
            || string.IsNullOrEmpty(nameValue))
        {
            return null;
        }

        return nameValue;
    }

    protected abstract string GetFilterKey();
}
