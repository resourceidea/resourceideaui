using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ResourceIdea.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when the subscription code is missing.
    /// </summary>
    public class SubscriptionCodeMissingException : Exception
    {
        public SubscriptionCodeMissingException()
        {
        }

        public SubscriptionCodeMissingException(string? message) : base(message)
        {
        }

        public SubscriptionCodeMissingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SubscriptionCodeMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
