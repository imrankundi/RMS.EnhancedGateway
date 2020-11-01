using System;

namespace RMS.Core.Exceptions
{
    public class ESException : Exception
    {
        public ESException() : base() { }
        public ESException(string message) : base(message) { }
        public ESException(string message, Exception innerException) : base(message, innerException) { }
    }
}
