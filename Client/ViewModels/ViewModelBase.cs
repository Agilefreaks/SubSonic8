using Caliburn.Micro;

namespace Client.ViewModels
{
    public abstract class ViewModelBase : Screen
    {
        public INavigationService NavigationService { get; private set; }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void GoBack()
        {
            NavigationService.GoBack();
        }

        public bool CanGoBack
        {
            get
            {
                return NavigationService.CanGoBack;
            }
        }
    }
}