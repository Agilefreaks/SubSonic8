using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Subsonic8.Menu;
using Windows.ApplicationModel.Activation;
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
            _container.RegisterSingleton(typeof(ISubsonicService), "subsonic", typeof(SubsonicService));            
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

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<MenuView>();
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
