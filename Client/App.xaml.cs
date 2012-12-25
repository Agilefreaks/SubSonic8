using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Main;
using Subsonic8.Playback;
using Subsonic8.Settings;
using Subsonic8.Shell;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
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

            _container.RegisterSingleton(typeof(ISubsonicService), "SubsonicService", typeof(SubsonicService));
            _container.RegisterSingleton(typeof(IStorageService), "StorageService", typeof(StorageService));
            _container.RegisterSingleton(typeof(IShellViewModel), "ShellViewModel", typeof(ShellViewModel));
            _container.RegisterSingleton(typeof(IPlaybackViewModel), "PlaybackViewModel", typeof(PlaybackViewModel));
            _container.RegisterSingleton(typeof(IDefaultBottomBarViewModel), "DefaultBottomBarViewModel", typeof(DefaultBottomBarViewModel));
            _container.RegisterSingleton(typeof(INotificationService), "NotificationService", typeof(ToastsNotificationService));

            SettingsPane.GetForCurrentView().CommandsRequested += (sender, args) => args.AddSetting<SettingsViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var result = _container.GetInstance(service, key);

            return result;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            base.PrepareViewFirst(rootFrame);
            InitializeServices();
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

            var shellView = (ShellView) RootFrame.Content;
            RegisterNavigationService(shellView.ShellFrame);

            var navigationService = _container.GetInstance(typeof (INavigationService), null) as INavigationService;
            navigationService.NavigateToViewModel<MainViewModel>();


            RegisterPlaybackViewModel();

            _shellViewModel = (IShellViewModel) _container.GetInstance(typeof (IShellViewModel), null);
            ViewModelBinder.Bind(_shellViewModel, shellView, null);
        }

        private void RegisterPlaybackViewModel()
        {
            var instance = _container.GetInstance(typeof(IPlaybackViewModel), "PlaybackViewModel");
            _container.RegisterInstance(typeof(PlaybackViewModel), "PlaybackViewModel", instance);
        }

        private void RegisterNavigationService(Frame shellFrame, bool treatViewAsLoaded = false)
        {
            _container.RegisterInstance(typeof(INavigationService), "NavigationService", new CustomFrameAdapter(shellFrame, treatViewAsLoaded));
        }

        private async void InitializeServices()
        {
            var subsonic8Configuration = await GetSubsonic8Configuration();

            var subsonicService = (ISubsonicService)GetInstance(typeof(ISubsonicService), null);
            subsonicService.Configuration = subsonic8Configuration.SubsonicServiceConfiguration;
            var notificationService = (INotificationService)GetInstance(typeof(INotificationService), null);
            notificationService.UseSound = subsonic8Configuration.ToastsUseSound;
        }

        private async Task<Subsonic8Configuration> GetSubsonic8Configuration()
        {
            var storageService = GetInstance(typeof (IStorageService), null) as IStorageService ?? new StorageService();
            var subsonic8Configuration = await storageService.Load<Subsonic8Configuration>() ?? new Subsonic8Configuration();
#if DEBUG
            const string baseUrl = "http://cristibadila.dynalias.com:33770/music/";
            subsonic8Configuration.SubsonicServiceConfiguration = new SubsonicServiceConfiguration
                {
                    BaseUrl = baseUrl,
                    Username = "media",
                    Password = "media"
                };
#endif
            return subsonic8Configuration;
        }
    }
}