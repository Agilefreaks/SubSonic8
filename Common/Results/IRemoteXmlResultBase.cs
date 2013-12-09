namespace Common.Results
{
    using System;
    using System.Threading.Tasks;
    using Common.Interfaces;

    public interface IRemoteXmlResultBase<out T> : IExtendedResult
    {
        T Result { get; }

        Func<Task<HttpStreamResult>> GetResourceFunc { get; set; }

        string RequestUrl { get; }

        string ResourcePath { get; }

        IRemoteXmlResultBase<T> OnSuccess(Action<T> onSuccess);

        new IRemoteXmlResultBase<T> WithErrorHandler(IErrorHandler errorHandler);
    }
}