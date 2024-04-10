using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Domain.Common.Exceptions;

public class InvalidEntityIdException : ResourceIdeaException
{
    public InvalidEntityIdException() { }

    public InvalidEntityIdException(string message) : base(message) { }

    public InvalidEntityIdException(string message, Exception innerException) : base(message, innerException) { }
}
