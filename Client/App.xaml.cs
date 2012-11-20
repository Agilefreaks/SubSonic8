using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Subsonic8.Main;
using Subsonic8.Settings;
using Subsonic8.Shell;
using WinRtUtility;
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
            _container.RegisterSingleton(typeof(IShellViewModel), "ShellViewModel", typeof(ShellViewModel));

            SettingsPane.GetForCurrentView().CommandsRequested += (sender, args) => args.AddSetting<SettingsViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var result = _container.GetInstance(service, key);

            // silly hack injecting NavigationService and SubsonicService
            if (result is ViewModelBase)
            {
                var subsonicService = (ISubsonicService)GetInstance(typeof(ISubsonicService), null);
                var navigationSerice = (INavigationService)GetInstance(typeof(INavigationService), null);
                var viewModelBase = result as ViewModelBase;
                viewModelBase.NavigationService = navigationSerice;
                viewModelBase.SubsonicService = subsonicService;
            }

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
            InitializeSubsonicService();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            StartApplication();
        }

        private void StartApplication()
        {
            DisplayRootView<ShellView>();

            var shellView = (ShellView) RootFrame.Content;
            RegisterNavigationService(shellView.ShellFrame);

            var navigationService = _container.GetInstance(typeof (INavigationService), null) as INavigationService;
            navigationService.UriFor<MainViewModel>().Navigate();

            _shellViewModel = (IShellViewModel) _container.GetInstance(typeof (IShellViewModel), null);
            ViewModelBinder.Bind(_shellViewModel, shellView, null);
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

        private void RegisterNavigationService(Frame shellFrame, bool treatViewAsLoaded = false)
        {
            _container.RegisterInstance(typeof(INavigationService), null, new CustomFrameAdapter(shellFrame, treatViewAsLoaded));
        }

        private async void InitializeSubsonicService()
        {
            var storageHelper = new ObjectStorageHelper<SubsonicServiceConfiguration>(StorageType.Roaming);
            var subsonicServiceConfiguration = await storageHelper.LoadAsync();

#if DEBUG
            subsonicServiceConfiguration = new SubsonicServiceConfiguration
            {
                ServiceUrl = "http://cristibadila.dynalias.com:33770/music/rest/{0}?u={1}&p={2}&v=1.8.0&c=SubSonic8",
                Username = "media",
                Password = "media"
            };
#endif

            var subsonicService = GetInstance(typeof(ISubsonicService), null) as ISubsonicService ?? new SubsonicService();
            subsonicService.Configuration = subsonicServiceConfiguration;
        }
    }
}