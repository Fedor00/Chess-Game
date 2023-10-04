using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Exceptions
{
    public class DataUpdateException : Exception
    {
        public DataUpdateException() : base()
        {
        }

        public DataUpdateException(string message) : base(message)
        {
        }

        public DataUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}