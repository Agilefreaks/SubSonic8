using System;

namespace Client.Common.Results
{
    public interface IErrorHandler
    {
        void HandleError(Exception error);
    }
}