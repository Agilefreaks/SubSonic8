namespace Client.Common.Exceptions
{
    using System;
    using Client.Common.Models.Subsonic;

    public class ApiException : Exception
    {
        public ApiException(string message)
            : base(message)
        {
        }

        public ApiException(Error error)
            : base(error.Message)
        {
        }
    }
}