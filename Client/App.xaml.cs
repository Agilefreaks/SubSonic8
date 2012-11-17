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
using Windows.UI.Xaml.Controls;

namespace Subsonic8
{
    public sealed partial class App
    {
        private WinRTContainer _container;

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

        protected override void PrepareViewFirst(Windows.UI.Xaml.Controls.Frame rootFrame)
        {
            base.PrepareViewFirst(rootFrame);
            InitializeSubsonicService();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<ShellView>();

            var shellView = (ShellView)RootFrame.Content ?? new ShellView();
            RegisterNavigationService(shellView.ShellFrame);

            var navigationService = _container.GetInstance(typeof(INavigationService), null) as INavigationService;
            navigationService.UriFor<MainViewModel>().Navigate();

            var shellViewModel = _container.GetInstance(typeof(IShellViewModel), null);
            ViewModelBinder.Bind(shellViewModel, shellView, null);
        }

        private void RegisterNavigationService(Frame shellFrame, bool treatViewAsLoaded = false)
        {
            _container.RegisterInstance(typeof(INavigationService), null, new CustomFrameAdapter(shellFrame, treatViewAsLoaded));
        }

        private async void InitializeSubsonicService()
        {
            var storageHelper = new ObjectStorageHelper<SubsonicServiceConfiguration>(StorageType.Roaming);
            SubsonicServiceConfiguration subsonicServiceConfiguration = await storageHelper.LoadAsync();

#if DEBUG
            subsonicServiceConfiguration = new SubsonicServiceConfiguration
            {
                ServiceUrl = "http://cristibadila.dynalias.com:33770/music/rest/{0}?u={1}&p={2}&v=1.8.0&c=SubSonic8",
                Username = "media",
                Password = "media"
            };
#endif

            ISubsonicService subsonicService = GetInstance(typeof(ISubsonicService), null) as ISubsonicService ?? new SubsonicService();
            subsonicService.Configuration = subsonicServiceConfiguration;
        }
    }
}
