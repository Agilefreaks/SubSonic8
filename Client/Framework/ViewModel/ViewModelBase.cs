using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Shell;

namespace Subsonic8.Framework.ViewModel
{
    public abstract class ViewModelBase : Screen, IViewModel
    {
        private INavigationService _navigationService;
        private ISubsonicService _subsonicService;
        private IBottomBarViewModel _bottomBar;

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

        public IBottomBarViewModel BottomBar
        {
            get
            {
                return _bottomBar;
            }

            set
            {
                _bottomBar = value;
                NotifyOfPropertyChange();
                SetShellBottomBar();
            }
        }

        public bool CanGoBack
        {
            get
            {
                return NavigationService != null && NavigationService.CanGoBack;
            }
        }

        public ObservableCollection<object> SelectedItems
        {
            get { return  ((IDefaultBottomBarViewModel) BottomBar).SelectedItems; }
        }

        public void GoBack()
        {
            NavigationService.GoBack();
        }

        protected ViewModelBase()
        {
            SubsonicService = IoC.Get<ISubsonicService>();
            NavigationService = IoC.Get<INavigationService>();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            SetBottomBar(IoC.Get<IShellViewModel>());
        }

        protected virtual void SetBottomBar(IShellViewModel shell)
        {
            BottomBar = IoC.Get<IDefaultBottomBarViewModel>();
        }

        private void SetShellBottomBar()
        {
            IoC.Get<IShellViewModel>().BottomBar = BottomBar;
        }
    }
}