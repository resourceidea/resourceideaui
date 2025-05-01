// ===============================================================================
// File: ParentComponent.cs
// Path: src\dev\Web\EastSeat.ResourceIdea.Web\Components\Shared\ParentComponent.cs
// Description: Stores information about the parent component of a child component used.
// ===============================================================================

namespace EastSeat.ResourceIdea.Web.Components.Shared;

/// <summary>
/// Stores information about the parent component of a child component used.
/// </summary>
public class ParentComponent
{
    /// <summary>
    /// View where the child component is used.
    /// </summary>
    public string View { get; set; } = string.Empty;

    /// <summary>
    /// Id on the url of the view where the child component is used.
    /// </summary>
    public string? Id { get; set; } = string.Empty;
}
