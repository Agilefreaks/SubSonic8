using Caliburn.Micro;
using Client.Common;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Playback;
using Subsonic8.Shell;
using Subsonic8.VideoPlayback;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MugenInjection;

namespace Subsonic8
{
    public sealed partial class App
    {
        private IShellViewModel _shellViewModel;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            Kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            Kernel.Load<CommonModule>();
            Kernel.Load<ClientModule>();
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
        }

        private ShellView GetShellView()
        {
            return (ShellView)RootFrame.Content;
        }

        private void RegisterNavigationService(Frame shellFrame, bool treatViewAsLoaded = false)
        {
            Kernel.Bind<INavigationService>().ToConstant(new CustomFrameAdapter(shellFrame, treatViewAsLoaded));
        }

        private void InstantiateRequiredSingletons()
        {
            Kernel.Get<IPlaybackViewModel>();
            Kernel.Get<IFullScreenVideoPlaybackViewModel>();
            Kernel.Get<IDefaultBottomBarViewModel>();
            Kernel.Get<IDialogNotificationService>();
        }

        private void BindShellViewModelToView(ShellView shellView)
        {
            _shellViewModel = Kernel.Get<IShellViewModel>();
            ViewModelBinder.Bind(_shellViewModel, shellView, null);
        }
    }
}