namespace Client.Common.Results
{
    using System;

    public interface IExtendedResult : IResultBase
    {
        #region Public Methods and Operators

        IExtendedResult OnSuccess(Action onSuccess);

        IExtendedResult WithErrorHandler(IErrorHandler errorHandler);

        #endregion
    }
}