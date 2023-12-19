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
            public static class UpdateClient
            {
                public const string ClientNotFound = "ClientNotFound";
                public const string ValidationFailure = "ValidationFailure";
                public const string EmptyClientId = "EmptyClientId";
                public const string MissingClientName = "MissingClientName";
                public const string InvalidColorCode = "InvalidColorCode";
                public const string EmptySubscriptionId = "EmptySubscriptionId";
            }
        }
    }
}