using Caliburn.Micro;
using Client.Common.Services;
using Subsonic8.BottomBar;

namespace Subsonic8.Framework.ViewModel
{
    public interface IViewModel : IScreen
    {
        INavigationService NavigationService { get; set; }

        ISubsonicService SubsonicService { get; set; }

        IBottomBarViewModel BottomBar { get; set; }

        bool CanGoBack { get; }

        void GoBack();
    }
}