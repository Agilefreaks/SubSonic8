using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Subsonic8.BottomBar;

namespace Subsonic8.Shell
{
    public interface IShellViewModel : IViewAware, IScreen
    {
        Uri Source { get; set; }

        IBottomBarViewModel BottomBar { get; set; }

        ISubsonicService SubsonicService { get; set; }
        
        Action<SearchResultCollection> NavigateToSearhResult { get; set; }
        
        Task PerformSubsonicSearch(string query);
    }
}