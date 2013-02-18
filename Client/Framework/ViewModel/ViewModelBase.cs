using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Shell;
using Action = System.Action;

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

        public IDialogNotificationService NotificationService { get; set; }

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
                NotifyOfPropertyChange(() => SelectedItems);
                SetShellBottomBar();
            }
        }

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return BottomBar != null ? BottomBar.SelectedItems : new ObservableCollection<object>();
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

        public Action UpdateDisplayName { get; set; }

        protected ViewModelBase()
        {
            SubsonicService = IoC.Get<ISubsonicService>();
            NavigationService = IoC.Get<INavigationService>();
            NotificationService = IoC.Get<IDialogNotificationService>();
            UpdateDisplayName = () => DisplayName = "Subsonic8";
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            SetBottomBar();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public void HandleError(Exception error)
        {
            NotificationService.Show(new DialogNotificationOptions
                                         {
                                             Message = error.ToString(),
                                         });
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            UpdateDisplayName();
            BottomBar.IsOnPlaylist = false;
        }

        protected virtual void SetBottomBar()
        {
            BottomBar = (IBottomBarViewModel)IoC.GetInstance(typeof(IDefaultBottomBarViewModel), "DefaultBottomBarViewModel");
        }

        private void SetShellBottomBar()
        {
            IoC.Get<IShellViewModel>().BottomBar = BottomBar;
        }
    }
}