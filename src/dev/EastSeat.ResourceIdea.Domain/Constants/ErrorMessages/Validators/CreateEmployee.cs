public static partial class Constants
{
    /// <summary>
    /// Error messages.
    /// </summary>
    public static partial class ErrorCodes
    {
        /// <summary>
        /// Error messages on validators.
        /// </summary>
        public static partial class Validators
        {
            /// <summary>
            /// Errors on CreateEmployee command.
            /// </summary>
            public static class CreateEmployee
            {
                public const string MissingUserId = "UserId is required.";
                public const string MissingJobPositionId = "JobPositionId is required.";
                public const string MissingSubscriptionId = "Subscription Id is required.";
            }
        }
    }
}