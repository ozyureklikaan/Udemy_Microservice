using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyMicroservices.Web.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException() : base()
        {
        }

        public UnauthorizeException(string message) : base(message)
        {
        }

        public UnauthorizeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
