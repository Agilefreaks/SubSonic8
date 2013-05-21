namespace Client.Common.Results
{
    using System;

    public interface IErrorHandler
    {
        #region Public Methods and Operators

        void HandleError(Exception error);

        #endregion
    }
}