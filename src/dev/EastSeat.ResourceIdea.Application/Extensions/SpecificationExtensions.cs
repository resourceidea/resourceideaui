using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Application.Extensions;

/// <summary>
/// Specification extensions.
/// </summary>
public static class SpecificationExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Func<T, bool> left, Func<T, bool> right) => x => left(x) && right(x);

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var compiledLeft = left.Compile();
        var compiledRight = right.Compile();

        return x => compiledLeft(x) || compiledRight(x);
    }

    public static Expression<Func<T, bool>> Not<T>(this Func<T, bool> spec) => x => !spec(x);

    public static Expression<Func<T, bool>> With<T, TProperty>(this Expression<Func<TProperty, bool>> spec, Expression<Func<T, TProperty>> selector)
    {
        var compiledSpec = spec.Compile();
        var compiledSelector = selector.Compile();

        return x => compiledSpec(compiledSelector(x));
    }
}
