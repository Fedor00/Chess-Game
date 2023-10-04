using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Exceptions
{
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException() : base()
        {
        }

        public InvalidMoveException(string message) : base(message)
        {
        }

        public InvalidMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}