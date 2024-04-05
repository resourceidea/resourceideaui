namespace EastSeat.ResourceIdea.Domain.Common.Constants;

public enum ErrorCodes
{
    /// <summary>Validation of the create tenant command failed.</summary>
    CreateTenantCommandValidationFailure,

    /// <summary>Validation of the command to update a tenant failed.</summary>
    UpdateTenantCommandValidationFailure,

    /// <summary>Resource being queried was not found.</summary>
    ResourceNotFound,
    CreateSubscriptionServiceCommandValidationFailure
}