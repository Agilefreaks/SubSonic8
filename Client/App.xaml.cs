using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Main;
using Subsonic8.Playback;
using Subsonic8.Shell;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8
{
    public sealed partial class App
    {
        private WinRTContainer _container;
        private IShellViewModel _shellViewModel;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            _container.RegisterSingleton(typeof(IWinRTWrappersService), "WinRTWrappersService", typeof(WinRTWrappersService));
            _container.RegisterSingleton(typeof(ISubsonicService), "SubsonicService", typeof(SubsonicService));
            _container.RegisterSingleton(typeof(IStorageService), "StorageService", typeof(StorageService));
            _container.RegisterSingleton(typeof(IShellViewModel), "ShellViewModel", typeof(ShellViewModel));
            _container.RegisterSingleton(typeof(IPlaybackViewModel), "PlaybackViewModel", typeof(PlaybackViewModel));
            _container.RegisterHandler(typeof(PlaybackViewModel), "PlaybackViewModel", container => container.GetInstance(typeof(IPlaybackViewModel), "PlaybackViewModel"));
            _container.RegisterSingleton(typeof(IDefaultBottomBarViewModel), "DefaultBottomBarViewModel", typeof(DefaultBottomBarViewModel));
            _container.RegisterSingleton(typeof(INotificationService), "NotificationService", typeof(ToastsNotificationService));
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            StartApplication();
        }

        protected async override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                StartApplication();
            }

            await _shellViewModel.PerformSubsonicSearch(args.QueryText);
        }

        private void StartApplication()
        {
            DisplayRootView<ShellView>();

            var shellView = GetShellView();

            RegisterNavigationService(shellView.ShellFrame);
            InstantiateRequiredSingletons();

            BindShellViewModelToView(shellView);

            ShowMainViewModel();
        }

        private ShellView GetShellView()
        {
            return (ShellView)RootFrame.Content;
        }

        private void RegisterNavigationService(Frame shellFrame, bool treatViewAsLoaded = false)
        {
            _container.RegisterInstance(typeof(INavigationService), "NavigationService", new CustomFrameAdapter(shellFrame, treatViewAsLoaded));
        }

        private void InstantiateRequiredSingletons()
        {
            _container.GetInstance(typeof(IPlaybackViewModel), "PlaybackViewModel");
            _container.GetInstance(typeof(IDefaultBottomBarViewModel), "DefaultBottomBarViewModel");
        }

        private void BindShellViewModelToView(ShellView shellView)
        {
            _shellViewModel = (IShellViewModel)_container.GetInstance(typeof(IShellViewModel), null);
            ViewModelBinder.Bind(_shellViewModel, shellView, null);
        }

        private void ShowMainViewModel()
        {
            var navigationService = _container.GetInstance(typeof(INavigationService), null) as INavigationService;
            navigationService.NavigateToViewModel<MainViewModel>();
        }
    }
}