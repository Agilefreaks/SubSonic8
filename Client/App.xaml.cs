using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Subsonic8.Menu;

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
            base.Configure();

            _container = new WinRTContainer(RootFrame);
            _container.RegisterWinRTServices();

            _container.RegisterSingleton(typeof(ISubsonicService), "subsonic", typeof(SubsonicService));

            _container.SettingsCommand<SettingsViewModel>(1, "Credentials");
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
            return typeof (MenuViewModel);
        }

//        protected override void OnSearchActivated(SearchActivatedEventArgs args)
//        {
//            DisplayRootView<SearchView>(args.QueryText);
//        }
//
//        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
//        {
//            // Normally wouldn't need to do this but need the _container to be initialised
//            Initialise();
//
//            _container.Instance(args.ShareOperation);
//
//            DisplayRootViewFor<ShareTargetViewModel>();
//        }
    }
}
