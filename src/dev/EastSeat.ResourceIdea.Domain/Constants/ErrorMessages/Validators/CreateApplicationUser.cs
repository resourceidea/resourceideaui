public static partial class Constants
{
    /// <summary>
    /// Error messages.
    /// </summary>
    public static partial class ErrorMessages
    {
        /// <summary>
        /// Error messages on validators.
        /// </summary>
        public static partial class Validators
        {
            /// <summary>
            /// Errors on CreateApplicationUser command.
            /// </summary>
            public static class CreateApplicationUser
            {
                public const string MissingFirstName = "First name is required.";
                public const string MissingLastName = "Last name is required.";
                public const string MissingSubscriptionId = "Subscription Id is required.";
                public const string MissingEmail = "Email is required.";
                public const string MissingPassword = "Password is required.";
            }
        }
    }
}
