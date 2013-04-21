using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Services;
using Subsonic8.Framework;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Main;
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
        private ApplicationExecutionState _previousExecutionState;
        private CustomFrameAdapter _navigationService;

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
            _previousExecutionState = ApplicationExecutionState.Terminated;
            StartApplication();
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                StartApplication();
            }

            _shellViewModel.SendSearchQueryMessage(args.QueryText);
        }

        protected override async void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        private async void StartApplication()
        {
            DisplayRootView<ShellView>();

            var shellView = GetShellView();

            RegisterNavigationService(shellView.ShellFrame);

            BindShellViewModelToView(shellView);

            RegisterPlayers();

            InstantiateRequiredSingletons();

            await LoadSettings();

            await RestoreLastViewOrGoToMain(shellView);
        }

        private ShellView GetShellView()
        {
            return (ShellView)RootFrame.Content;
        }

        private void RegisterNavigationService(Frame shellFrame, bool treatViewAsLoaded = false)
        {
            _navigationService = new CustomFrameAdapter(shellFrame, treatViewAsLoaded);
            Kernel.Bind<INavigationService>().ToConstant(_navigationService);
            Kernel.Bind<ICustomFrameAdapter>().ToConstant(_navigationService);
        }

        private void RegisterPlayers()
        {
            var playerManagementService = Kernel.Get<IPlayerManagementService>();
            playerManagementService.RegisterAudioPlayer(_shellViewModel);
            playerManagementService.RegisterVideoPlayer(Kernel.Get<IEmbededVideoPlaybackViewModel>());
            playerManagementService.RegisterVideoPlayer(Kernel.Get<IFullScreenVideoPlaybackViewModel>());
        }

        private void InstantiateRequiredSingletons()
        {
            Kernel.Get<IPlaybackViewModel>();
        }

        private void BindShellViewModelToView(ShellView shellView)
        {
            _shellViewModel = Kernel.Get<IShellViewModel>();

            ViewModelBinder.Bind(_shellViewModel, shellView, null);
        }

        private async Task LoadSettings()
        {
            await Kernel.Get<ISettingsHelper>().LoadSettings();
        }

        private async Task RestoreLastViewOrGoToMain(ShellView shellView)
        {
            if (_previousExecutionState == ApplicationExecutionState.Terminated)
            {
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException)
                {
                }
            }

            SuspensionManager.RegisterFrame(shellView.ShellFrame, "MainFrame");

            if (shellView.ShellFrame.SourcePageType == null)
            {
                _navigationService.NavigateToViewModel<MainViewModel>();
            }
        }
    }
}