using Caliburn.Micro;

namespace Client.Common
{
    public abstract class ViewModelBase : Screen, IViewModel
    {
        private INavigationService _navigationService;
        private ISubsonicService _subsonicService;

        public INavigationService NavigationService
        {
            get
            {
                return _navigationService;
            }

            set
            {
                _navigationService = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanGoBack);
            }
        }

        public ISubsonicService SubsonicService
        {
            get
            {
                return _subsonicService;
            }

            set
            {
                _subsonicService = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CanGoBack
        {
            get
            {
                return NavigationService != null && NavigationService.CanGoBack;
            }
        }

        public void GoBack()
        {
            NavigationService.GoBack();
        }
    }
}