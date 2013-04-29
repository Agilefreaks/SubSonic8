using Client.Common.Results;
using Client.Common.Services;

namespace Subsonic8.Framework.ViewModel
{
    public interface ISongLoader : IErrorHandler
    {
        ISubsonicService SubsonicService { get; set; }
    }
}