using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Exceptions
{
    public class AlreadyPlayingException : Exception
    {
        public AlreadyPlayingException() : base()
        {
        }

        public AlreadyPlayingException(string message) : base(message)
        {
        }

        public AlreadyPlayingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}