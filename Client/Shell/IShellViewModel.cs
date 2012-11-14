using Caliburn.Micro;
using Subsonic8.Messages;

namespace Subsonic8.Shell
{
    public interface IShellViewModel : IViewAware, IScreen, IHandle<PlayFile>
    {    
    }
}