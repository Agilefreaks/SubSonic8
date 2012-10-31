using Caliburn.Micro;

namespace Client.Common
{
    public abstract class ViewModelBase : Screen
    {
        protected INavigationService NavigationService { get; set; }

        public bool CanGoBack
        {
            get
            {
                return NavigationService.CanGoBack;
            }
        }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void GoBack()
        {
            NavigationService.GoBack();
        }
    }
}