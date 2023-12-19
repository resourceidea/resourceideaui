public static partial class Constants
{
    /// <summary>
    /// Error messages.
    /// </summary>
    public static partial class ErrorCodes
    {
        /// <summary>
        /// Error messages on commands.
        /// </summary>
        public static partial class Commands
        {
            /// <summary>
            /// Errors on CreateApplicationUser command.
            /// </summary>
            public static class CreateApplicationUsers
            {
                public const string InvalidApplicationUserId = "Application User created with an invalid UserId.";
                public const string UsernameExists = "Username already exists.";
                public const string UserRegistrationFailed = "User registration failed.";
                public const string EmailExists = "Email already exists.";
            }
        }
    }
}