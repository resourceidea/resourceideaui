namespace EastSeat.ResourceIdea.Application.Enums;

public enum ErrorCode
{

    /// <summary>
    /// Represents no error.
    /// </summary>
    None,
    
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
    NotFound,

    /// <summary>
    /// Validation of the command failed.
    /// </summary>
    CommandValidationFailure,

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

    /// <summary>
    /// Failure to get the expected repository type by the unit of work.
    /// </summary>
    GetRepositoryFailure,

    /// <summary>
    /// Empty entity returned from the repository.
    /// </summary>
    EmptyEntity,

    /// <summary>
    /// Validation of the login command failed.
    /// </summary>
    LoginCommandValidationFailure,

    /// <summary>
    /// Login failed because user was not found.
    /// </summary>
    UserNotFound,

    /// <summary>
    /// Login failed.
    /// </summary>
    LoginFailed,

    /// <summary>
    /// Bad request made to the application.
    /// </summary>
    BadRequest,

    /// <summary>
    /// Query to the data store failed.
    /// </summary>
    DataStoreQueryFailure,
    
    /// <summary>
    /// Command to the data store failed.
    /// </summary>
    DataStoreCommandFailure,
}