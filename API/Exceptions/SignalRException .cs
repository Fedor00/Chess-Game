using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Exceptions
{
    public class SignalRException : Exception
    {
        public SignalRException() : base()
        {
        }

        public SignalRException(string message) : base(message)
        {
        }

        public SignalRException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}