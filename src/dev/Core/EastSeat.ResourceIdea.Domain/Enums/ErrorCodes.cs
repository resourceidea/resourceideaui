namespace EastSeat.ResourceIdea.Domain.Enums;

public enum ErrorCodes
{
    /// <summary>
    /// Validation of the create tenant command failed.
    /// </summary>
    CreateTenantCommandValidationFailure,

    /// <summary>
    /// Validation of the command to update a tenant failed.
    /// </summary>
    UpdateTenantCommandValidationFailure,

    /// <summary>
    /// Resource being queried was not found.
    /// </summary>
    ResourceNotFound,

    /// <summary>
    /// Validation of the command to create a subscription service failed.
    /// </summary>
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
    /// Querying of an item from the data store failed.
    /// </summary>
    ItemNotFound,

    /// <summary>
    /// Cancellation of a subscription failed.
    /// </summary>
    SubscriptionCancellationFailure,

    /// <summary>
    /// Validation of the command to create a client failed.
    /// </summary>
    CreateClientCommandValidationFailure,

    /// <summary>
    /// Validation of the command to update a client failed.
    /// </summary>
    UpdateClientCommandValidationFailure,

    /// <summary>
    /// Validation of the complete engagement command failed.
    /// </summary>
    CompleteEngagementCommandValidationFailure,
    
    /// <summary>
    /// Validation of the start engagement command failed.
    /// </summary>
    StartEngagementCommandValidationFailure,

    /// <summary>
    /// Validation of the update engagement command failed.
    /// </summary>
    UpdateEngagementCommandValidationFailure,

    /// <summary>
    /// Validation of the cancel subscription command failed.
    /// </summary>
    SubscriptionCancelationFailure,
}