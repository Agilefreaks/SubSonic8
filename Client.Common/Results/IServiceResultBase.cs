namespace Client.Common.Results
{
    using System;
    using System.Threading.Tasks;
    using Client.Common.Services.DataStructures.SubsonicService;
    using global::Common.Interfaces;
    using global::Common.Results;

    public interface IServiceResultBase<out T> : IExtendedResult
    {
        #region Public Properties

        ISubsonicServiceConfiguration Configuration { get; }

        string RequestUrl { get; }

        Func<Task<HttpStreamResult>> Response { get; set; }

        T Result { get; }

        string ViewName { get; }

        #endregion

        #region Public Methods and Operators

        IServiceResultBase<T> OnSuccess(Action<T> onSuccess);

        new IServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler);

        #endregion
    }
}