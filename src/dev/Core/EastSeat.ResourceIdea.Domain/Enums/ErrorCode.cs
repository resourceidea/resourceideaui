// =========================================================================================
// File: ErrorCode.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Enums\ErrorCode.cs
// Description: Enum representing various error codes used in the application.
// =========================================================================================

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

    /// <summary>
    /// Empty entity returned from repository on closing and engagement task.
    /// </summary>
    EmptyEntityOnCloseEngagementTask,

    /// <summary>
    /// Empty entity returned from repository on blocking and engagement task.
    /// </summary>
    EmptyEntityOnBlockEngagementTask,

    /// <summary>
    /// Empty entity returned from repository on assignment engagement task.
    /// </summary>
    EmptyEntityOnAssignEngagementTask,

    /// <summary>
    /// Empty entity returned from repository on creating a subscription.
    /// </summary>
    EmptyEntityOnCreateSubscriptionService,

    /// <summary>
    /// Empty entity returned from repository on updating a subscription.
    /// </summary>
    EmptyEntityOnUpdateSubscriptionService,

    /// <summary>
    /// Database insert operation failed on attempt to create department.
    /// </summary>
    DbInsertFailureOnCreateDepartment,

    /// <summary>
    /// Failure on querying for department.
    /// </summary>
    QueryForDepartmentFailure,

    /// <summary>
    /// Empty entity returned from repository on update department.
    /// </summary>
    EmptyEntityOnUpdateDepartment,

    /// <summary>
    /// Database update operation failed on attempt to update department.
    /// </summary>
    DbUpdateFailureOnUpdateDepartment,

    /// <summary>
    /// Database insert operation failed on attempt to create job position.
    /// </summary>
    DbInsertFailureOnCreateJobPosition,

    /// <summary>
    /// Invalid create job position command.
    /// </summary>
    InvalidCreateJobPositionCommand,

    /// <summary>
    /// Database update operation failed on attempt to update job position.
    /// </summary>
    DbUpdateFailureOnUpdateJobPosition,

    /// <summary>
    /// Empty entity returned from repository on update job position.
    /// </summary>
    EmptyEntityOnUpdateJobPosition,

    /// <summary>
    /// Indicates a failure to insert a new employee record into the database.
    /// </summary>
    DbInsertFailureOnAddNewEmployee,

    /// <summary>
    /// Indicates a failure to insert a new application user into the database.
    /// </summary>
    DbInsertFailureOnAddApplicationUser,

    /// <summary>
    /// Handles the failure of adding an application user.
    /// </summary>
    AddApplicationUserFailure,

    /// <summary>
    /// Represents a failure that occurred while attempting to delete an application user.
    /// </summary>
    DeleteApplicationUserFailure,

    /// <summary>
    /// Indicates that the operation is not supported.
    /// </summary>
    UnSupportedOperation,

    /// <summary>
    /// Failure on the tenant employees query.
    /// </summary>
    FailureOnTenantEmployeesQuery,

    /// <summary>
    /// Indicates that the application user was not found.
    /// </summary>
    ApplicationUserNotFound,

    /// <summary>
    /// Indicates that the employee was not found.
    /// </summary>
    EmployeeNotFound,

    /// <summary>
    /// Indicates operation to update an employee failed.
    /// </summary>
    DbUpdateFailureOnUpdateEmployee,

    /// <summary>
    /// Indicates operation has not been implemented yet.
    /// </summary>
    NotImplemented,

    /// <summary>
    /// Indicates that the employee query validation failed.
    /// </summary>
    EmployeeQueryValidationFailure,
    FailureOnTenantClientsSpecification,
    DbInsertFailureOnAddClient,
    ClientAlreadyExists,
    DatabaseError,
    ClientQueryValidationFailure,

    /// <summary>
    /// Empty entity returned from repository on create work item.
    /// </summary>
    EmptyEntityOnCreateWorkItem,
}