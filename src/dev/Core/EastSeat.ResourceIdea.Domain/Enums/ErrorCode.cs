namespace EastSeat.ResourceIdea.Domain.Enums;

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
    EmptyEntityOnCreateClient,

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

    /// <summary>
    /// Empty entity returned from the repository on create department.
    /// </summary>
    EmptyEntityOnCreateDepartment,

    /// <summary>
    /// Empty entity returned from repository on create tenant.
    /// </summary>  
    EmptyEntityOnCreateTenant,

    /// <summary>
    /// Empty entity returned from repository on update tenant.
    /// </summary>
    EmptyEntityOnUpdateTenant,

    /// <summary>
    /// Empty entity returned from repository on update client.
    /// </summary>
    EmptyEntityOnUpdateClient,

    /// <summary>
    /// Empty entity returned from repository on cancel engagement.
    /// </summary>
    EmptyEntityOnCancelEngagement,

    /// <summary>
    /// Empty entity returned from repository on complete engagement.
    /// </summary>
    EmptyEntityOnCompleteEngagement,

    /// <summary>
    /// Empty entity returned from repository on create engagement.
    /// </summary>
    EmptyEntityOnCreateEngagement,

    /// <summary>
    /// Empty entity returned from repository on start engagement.
    /// </summary>
    EmptyEntityOnStartEngagement,

    /// <summary>
    /// Empty entity returned from repository on update engagement.
    /// </summary>
    EmptyEntityOnUpdateEngagement,

    /// <summary>
    /// Empty entity returned from repository on create engagement task.
    /// </summary>
    EmptyEntityOnCreateEngagementTask,

    /// <summary>
    /// Empty entity returned from repository on start engagement task.
    /// </summary>
    EmptyEntityOnStartEngagementTask,

    /// <summary>
    /// Empty entity returned from repository on update engagement task.
    /// </summary>
    EmptyEntityOnUpdateEngagementTask,

    /// <summary>
    /// Empty entity returned from repository on remove engagement task.
    /// </summary>
    EmptyEntityOnRemoveEngagementTask,
    EmptyEntityOnCloseEngagementTask,
    EmptyEntityOnBlockEngagementTask,
    EmptyEntityOnAssignEngagementTask,
}