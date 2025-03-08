// ----------------------------------------------------------------------------------
// File: DepartmentListModel.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Departments\Models\DepartmentListModel.cs
// Description: Model representing department listing data.
// ----------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace EastSeat.ResourceIdea.Domain.Departments.Models;

/// <summary>
/// Model representing department listing data.
/// </summary>
public sealed record DepartmentListModel
{
    public ReadOnlyCollection<DepartmentModel> Value { get; init; }

    public DepartmentListModel(IEnumerable<DepartmentModel> departments)
    {
        Value = new ReadOnlyCollection<DepartmentModel>([.. departments]);
    }
}
