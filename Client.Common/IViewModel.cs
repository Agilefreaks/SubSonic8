using Caliburn.Micro;

namespace Client.Common
{
    public interface IViewModel : IScreen
    {
        INavigationService NavigationService { get; set; }

        ISubsonicService SubsonicService { get; set; }

        bool CanGoBack { get; }

        void GoBack();
    }
}