namespace Common.Exceptions
{
    using System;
    using Common.Interfaces;

    public class ApiException : Exception
    {
        public ApiException(string message)
            : base(message)
        {
        }

        public ApiException(IError error)
            : base(error.Message)
        {
        }
    }
}