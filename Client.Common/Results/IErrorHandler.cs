namespace Client.Common.Results
{
    using System;
    using System.Threading.Tasks;

    public interface IErrorHandler
    {
        #region Public Methods and Operators

        Task HandleError(Exception error);

        #endregion
    }
}