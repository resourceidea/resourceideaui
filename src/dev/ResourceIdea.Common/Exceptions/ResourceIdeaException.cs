namespace ResourceIdea.Common.Exceptions
{
    /// <summary>
    /// Exceptions thrown by ResourceIdea.
    /// </summary>
    [Serializable]
    public class ResourceIdeaException : Exception
    {
        /// <summary>
        /// Gets or sets the error code associated with the exception.
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceIdeaException"/> class.
        /// </summary>
        /// <param name="httpStatusCode">HTTP status code.</param>
        /// <param name="errorCode">Error code.</param>
        /// <param name="message">Detailed message about the exception.</param>
        public ResourceIdeaException(HttpStatusCode httpStatusCode, ErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceIdeaException"/> class.
        /// </summary>
        /// <param name="httpStatusCode">HTTP status code.</param>
        /// <param name="errorCode">Error code.</param>
        /// <param name="message">Detailed message about the exception.</param>
        /// <param name="inner">Inner exception.</param>
        public ResourceIdeaException(HttpStatusCode httpStatusCode, ErrorCode errorCode, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }
    }
}
