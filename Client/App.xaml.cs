namespace Subsonic8
{
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common;
    using Client.Common.Services;
    using MugenInjection;
    using Subsonic8.Framework;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Main;
    using Subsonic8.Playback;
    using Subsonic8.Shell;
    using Subsonic8.VideoPlayback;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class App
    {
        #region Fields

        private CustomFrameAdapter _navigationService;

        private ApplicationExecutionState _previousExecutionState;

        private IShellViewModel _shellViewModel;

        #endregion

        #region Constructors and Destructors

        public App()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        protected override void Configure()
        {
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

        protected override async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        private void BindShellViewModelToView(ShellView shellView)
        {
            _shellViewModel = Kernel.Get<IShellViewModel>();

            ViewModelBinder.Bind(_shellViewModel, shellView, null);
        }

        private ShellView GetShellView()
        {
            return (ShellView)RootFrame.Content;
        }

        private void InstantiateRequiredSingletons()
        {
            Kernel.Get<IPlaybackViewModel>();
            Kernel.Get<INotificationsHelper>();
        }

        private async Task LoadSettings()
        {
            await Kernel.Get<ISettingsHelper>().LoadSettings();
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

        #endregion
    }
}