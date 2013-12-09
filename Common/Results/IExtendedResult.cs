namespace Common.Results
{
    using System;
    using Common.Interfaces;

    public interface IExtendedResult : IResultBase
    {
        #region Public Methods and Operators

        IExtendedResult OnSuccess(Action onSuccess);

        IExtendedResult WithErrorHandler(IErrorHandler errorHandler);

        #endregion
    }
}