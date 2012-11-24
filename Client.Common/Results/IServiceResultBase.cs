using System;
using System.IO;
using System.Threading.Tasks;
using Client.Common.Services;

namespace Client.Common.Results
{
    public interface IServiceResultBase<out T> : IResultBase
    {
        ISubsonicServiceConfiguration Configuration { get; }

        Func<Task<Stream>> Response { get; set; }

        T Result { get; }

        string ViewName { get; }

        string RequestUrl { get; }
    }
}