using Caliburn.Micro;
using Client.Common.Services;

namespace Client.Common.ViewModels
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