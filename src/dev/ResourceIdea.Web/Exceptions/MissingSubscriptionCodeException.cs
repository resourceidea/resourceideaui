namespace ResourceIdea.Web.Exceptions
{
    /// <summary>
    /// Missing subscription code exception.
    /// </summary>
    internal class MissingSubscriptionCodeException : ResourceIdeaException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingSubscriptionCodeException"/> class.
        /// </summary>
        public MissingSubscriptionCodeException() : base(
            HttpStatusCode.Forbidden,
            ErrorCode.SubscriptionCodeMissing,
            StringConstants.MISSING_SUBSCRIPTION_CODE_ERROR){}
    }
}
