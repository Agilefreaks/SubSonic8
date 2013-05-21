using System;
using System.Threading.Tasks;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public interface IServiceResultBase<out T> : IExtendedResult
    {
        ISubsonicServiceConfiguration Configuration { get; }

        Func<Task<HttpStreamResult>> Response { get; set; }

        T Result { get; }

        string ViewName { get; }

        string RequestUrl { get; }

        IServiceResultBase<T> OnSuccess(Action<T> onSuccess);

        new IServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler);
    }
}