using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Client.Common.Results
{
    public interface IServiceResultBase : IResultBase
    {
        ISubsonicServiceConfiguration Configuration { get; }
        
        string ViewName { get; }

        Func<Task<Stream>> Response { get; set; }

        string RequestUrl { get; }
    }
}