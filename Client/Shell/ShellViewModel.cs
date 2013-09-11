namespace Subsonic8.Shell
{
    using System;
    using System.Threading.Tasks;
    using BugFreak;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Helpers;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Client.Common.Services;
    using MugenInjection.Attributes;
    using Subsonic8.BottomBar;
    using Subsonic8.ErrorDialog;
    using Subsonic8.Framework;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Framework.Services;
    using Subsonic8.Main;
    using Subsonic8.Playback;
    using Subsonic8.Search;
    using Subsonic8.Settings;
    using Subsonic8.VideoPlayback;
    using Windows.UI.Core;
    using Windows.UI.Xaml;

    public class ShellViewModel : Screen, IShellViewModel
    {
        #region Fields

        private IEventAggregator _eventAggregator;

        private IBottomBarViewModel _bottomBar;

        private IPlayerControls _playerControls;

        private Uri _source;

        #endregion

        #region Constructors and Destructors

        public ShellViewModel()
        {
            DisplayName = "Subsonic8";
        }

        #endregion

        #region Public Properties

        public IBottomBarViewModel BottomBar
        {
            get
            {
                return _bottomBar;
            }

            set
            {
                if (_bottomBar == value)
                {
                    return;
                }

                _bottomBar = value;
                NotifyOfPropertyChange();
            }
        }

        public Action<SearchResultCollection> NavigateToSearchResult { get; set; }

        public IPlayerControls PlayerControls
        {
            get
            {
                return _playerControls;
            }

            set
            {
                _playerControls = value;
                NotifyOfPropertyChange();
            }
        }

        public Uri Source
        {
            get
            {
                return _source;
            }

            set
            {
                _source = value;
                NotifyOfPropertyChange();
            }
        }

        [Inject]
        public IEventAggregator EventAggregator
        {
            get
            {
                return _eventAggregator;
            }

            set
            {
                _eventAggregator = value;
                _eventAggregator.Subscribe(this);
            }
        }

        [Inject]
        public IDialogNotificationService DialogNotificationService { get; set; }

        [Inject]
        public ICustomFrameAdapter NavigationService { get; set; }

        [Inject]
        public IToastNotificationService NotificationService { get; set; }

        [Inject]
        public IStorageService StorageService { get; set; }

        [Inject]
        public ISubsonicService SubsonicService { get; set; }

        [Inject]
        public IWinRTWrappersService WinRTWrappersService { get; set; }

        [Inject]
        public IErrorDialogViewModel ErrorDialogViewModel { get; set; }

        [Inject]
        public IResourceService ResourceService { get; set; }

        [Inject]
        public ISettingsHelper SettingsHelper { get; set; }

        [Inject]
        public IoCService IoCService { get; set; }

        [Inject]
        public IPlayerManagementService PlayerManagementService { get; set; }

        [Inject]
        public IEmbeddedVideoPlaybackViewModel EmbeddedVideoPlaybackViewModel { get; set; }

        [Inject]
        public IFullScreenVideoPlaybackViewModel FullScreenVideoPlaybackViewModel { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Handle(ChangeBottomBarMessage message)
        {
            BottomBar = message.BottomBarViewModel;
        }

        public async Task HandleError(Exception error)
        {
            await new MessageDialogResult(error.ToString(), "Oops...").Execute();
        }

        public void Pause()
        {
            _playerControls.Pause();
        }

        public void Play(PlaylistItem item, object options = null)
        {
            Source = item.Uri;
            _playerControls.Play();
        }

        public void Resume()
        {
            _playerControls.Play();
        }

        public void SendSearchQueryMessage(string query)
        {
            NavigationService.NavigateToViewModel<SearchViewModel>(query);
        }

        public void SongEnded()
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public void SongFailed(ExceptionRoutedEventArgs eventArgs)
        {
            _eventAggregator.Publish(new PlayFailedMessage(eventArgs.ErrorMessage, eventArgs.OriginalSource));
        }

        public void Stop()
        {
            _playerControls.Stop();
        }

        #endregion

        #region Methods

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            WinRTWrappersService.RegisterSearchQueryHandler((sender, args) => SendSearchQueryMessage(args.QueryText));
            WinRTWrappersService.RegisterSettingsRequestedHandler(
                (sender, args) => args.AddSetting<SettingsViewModel>());
            WinRTWrappersService.RegisterSettingsRequestedHandler(
                (sender, args) => args.AddSetting<PrivacyPolicyViewModel>());
            WinRTWrappersService.RegisterMediaControlHandler(new MediaControlHandler(_eventAggregator));

            PlayerControls = (IPlayerControls)view;

            var dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    HookBugFreak();

                    RegisterPlayers();

                    InstantiateRequiredSingletons();

                    await LoadSettings();

                    await RestoreLastViewOrGoToMain((ShellView)view);
                });
        }

        private void RegisterPlayers()
        {
            PlayerManagementService.RegisterAudioPlayer(this);
            PlayerManagementService.RegisterVideoPlayer(EmbeddedVideoPlaybackViewModel);
            PlayerManagementService.RegisterVideoPlayer(FullScreenVideoPlaybackViewModel);
        }

        private void InstantiateRequiredSingletons()
        {
            IoCService.Get<IPlaybackViewModel>();
            IoCService.Get<INotificationsHelper>();
        }

        private async Task LoadSettings()
        {
            await SettingsHelper.LoadSettings();
        }

        private async Task RestoreLastViewOrGoToMain(ShellView shellView)
        {
            try
            {
                await SuspensionManager.RestoreAsync();
            }
            catch (SuspensionManagerException)
            {
            }

            SuspensionManager.RegisterFrame(shellView.ShellFrame, "MainFrame");

            if (shellView.ShellFrame.SourcePageType == null ||
                shellView.ShellFrame.SourcePageType == typeof(ErrorDialogView))
            {
                NavigationService.NavigateToViewModel<MainViewModel>();
            }
        }

        private void HookBugFreak()
        {
            var apiKey = ResourceService.GetStringResource("BugFreakCredentials/ApiKey");
            var token = ResourceService.GetStringResource("BugFreakCredentials/Token");
            BugFreak.Hook(apiKey, token, Application.Current);
        }

        #endregion
    }
}