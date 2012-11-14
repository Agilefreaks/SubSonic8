using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Subsonic8.Main;
using Subsonic8.Settings;
using Subsonic8.Shell;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8
{
    public sealed partial class App
    {
        private SimpleContainer _container;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            base.Configure();

            _container = new SimpleContainer();

            RegisterWinRTServices(RootFrame);

            _container.RegisterSingleton(typeof(ISubsonicService), "subsonic", typeof(SubsonicService));
            _container.RegisterSingleton(typeof(IShellViewModel), "ShellViewModel", typeof(ShellViewModel));

            _container.SettingsCommand<SettingsViewModel>(1, "Credentials");
        }

        protected override void OnLaunched(Windows.ApplicationModel.Activation.LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            var shellView = (ShellView)RootFrame.Content;
            BindShellViewModel(shellView);
            var navigationService = RegisterNavigationServiceForShellFrame(shellView);
            navigationService.NavigateToViewModel<MainViewModel>();
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

        protected override Type GetDefaultViewModel()
        {
            return typeof(ShellViewModel);
        }

        private void BindShellViewModel(DependencyObject shellView)
        {
            var shellViewModel = _container.GetInstance(typeof (IShellViewModel), null);
            ViewModelBinder.Bind(shellViewModel, shellView, null);
        }

        private INavigationService RegisterNavigationServiceForShellFrame(ShellView shellView)
        {
            var navigationService = new NavigationService(shellView.ShellFrame);
            _container.RegisterInstance(typeof(INavigationService), null, navigationService);
            
            return navigationService;
        }

        private void RegisterWinRTServices(Frame rootFrame)
        {
            _container.RegisterInstance(typeof(SimpleContainer), null, _container);

            if (!_container.HasHandler(typeof(IEventAggregator), null))
            {
                _container.RegisterSingleton(typeof(IEventAggregator), null, typeof(EventAggregator));
            }
            if (!_container.HasHandler(typeof(ShareSourceService), null))
            {
                _container.RegisterInstance(typeof(ShareSourceService), null, new ShareSourceService(rootFrame));
            }

            if (!_container.HasHandler(typeof(CallistoSettingsService), null))
            {
                _container.RegisterInstance(typeof(CallistoSettingsService), null, new CallistoSettingsService());
            }
        }
    }
}
