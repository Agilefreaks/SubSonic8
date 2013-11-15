namespace SubEchoNest.Results
{
    using System;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Common.Results;
    using SubEchoNest;

    public interface IEchoNestResultBase<T> : IExtendedResult
    {
        IConfiguration Configuration { get; }

        string RequestUrl { get; }

        Func<Task<HttpStreamResult>> Response { get; set; }

        T Result { get; set; }

        string MethodName { get; }

        IEchoNestResultBase<T> OnSuccess(Action<T> onSuccess);

        new IEchoNestResultBase<T> WithErrorHandler(IErrorHandler errorHandler);
    }
}