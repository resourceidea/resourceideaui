// ----------------------------------------------------------------------------------
// File: ValidationResponse.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Types\ValidationResponse.cs
// Description: Validation result type.
// ----------------------------------------------------------------------------------

namespace EastSeat.ResourceIdea.Domain.Types;

/// <summary>
/// Validation result.
/// </summary>
/// <param name="IsValid">Validation is successfull.</param>
/// <param name="ValidationFailureMessages">Message(s) on validation failure, otherwise empty.</param>
public sealed record ValidationResponse(bool IsValid, IEnumerable<string> ValidationFailureMessages);
