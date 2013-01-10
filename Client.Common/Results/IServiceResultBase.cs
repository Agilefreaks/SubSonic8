using System;
using System.Threading.Tasks;
using Client.Common.Services;

namespace Client.Common.Results
{
    public interface IServiceResultBase<T> : IResultBase
    {
        ISubsonicServiceConfiguration Configuration { get; }

        Func<Task<HttpStreamResult>> Response { get; set; }

        T Result { get; }

        string ViewName { get; }

        string RequestUrl { get; }

        ServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler);
    }
}