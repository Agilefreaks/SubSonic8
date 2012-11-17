using System;
using Caliburn.Micro;
using Client.Common;
using Subsonic8.Messages;

namespace Subsonic8.Shell
{
    public interface IShellViewModel : IViewAware, IScreen, IHandle<PlayFile>
    {
        Uri Source { get; set; }

        ISubsonicService SubsonicService { get; set; }
    }
}