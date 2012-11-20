using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.Messages;

namespace Subsonic8.Shell
{
    public interface IShellViewModel : IViewAware, IScreen, IHandle<PlayFile>
    {
        Uri Source { get; set; }

        ISubsonicService SubsonicService { get; set; }
        
        Action<SearchResultCollection> NavigateToSearhResult { get; set; }

        Task PerformSubsonicSearch(string query);
    }
}