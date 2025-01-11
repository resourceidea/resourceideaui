using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Departments.Entities;

using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Specifications;

public sealed class DepartmentNameSpecification (Dictionary<string, string> filters)
    : BaseStringSpecification<Department>(filters)
{
    protected override Expression<Func<Department, bool>> GetExpression()
    {
        string departmentNameFilter = _filters[GetFilterKey()];
        if (string.IsNullOrEmpty(departmentNameFilter) || string.IsNullOrWhiteSpace(departmentNameFilter))
        {
            return department => true;
        }

        return department => department.Name.Contains(departmentNameFilter);
    }

    protected override string GetFilterKey() => "name";
}
