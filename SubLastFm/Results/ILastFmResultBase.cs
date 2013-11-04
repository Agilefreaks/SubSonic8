namespace SubLastFm.Results
{
    using System;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Common.Results;

    public interface ILastFmResultBase<T> : IExtendedResult
    {
        IConfiguration Configuration { get; }

        string RequestUrl { get; }

        Func<Task<HttpStreamResult>> Response { get; set; }

        T Result { get; set; }

        string MethodName { get; }

        ILastFmResultBase<T> OnSuccess(Action<T> onSuccess);

        new ILastFmResultBase<T> WithErrorHandler(IErrorHandler errorHandler);
    }
}