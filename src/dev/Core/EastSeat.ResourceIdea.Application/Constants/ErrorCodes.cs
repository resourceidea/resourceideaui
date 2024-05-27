namespace EastSeat.ResourceIdea.Application.Constants;

public enum ErrorCodes
{
    /// <summary>Validation of the create tenant command failed.</summary>
    CreateTenantCommandValidationFailure,

    /// <summary>Validation of the command to update a tenant failed.</summary>
    UpdateTenantCommandValidationFailure,

    /// <summary>Resource being queried was not found.</summary>
    ResourceNotFound,

    /// <summary>Validation of the command to create a subscription service failed.</summary>
    CreateSubscriptionServiceCommandValidationFailure,

    /// <summary>
    /// Validation of the command to update a subscription service failed.
    /// </summary>
    UpdateSubscriptionServiceCommandValidationFailure,

    /// <summary>
    /// Suspension of a subscription failed.
    /// </summary>
    SubscriptionSuspensionFailure,

    /// <summary>
    /// Querying of and item from the data store failed.
    /// </summary>
    ItemNotFound,

    /// <summary>
    /// Cancelation of a subscription failed.
    /// </summary>
    SubscriptionCancelationFailure,

    /// <summary>
    /// Validation of the command to create a client failed.
    /// </summary>
    CreateClientCommandValidationFailure,
}