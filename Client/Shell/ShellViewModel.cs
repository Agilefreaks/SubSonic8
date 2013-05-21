namespace Subsonic8.Shell
{
    using System;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Helpers;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Client.Common.Services;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Framework.Services;
    using Subsonic8.Search;
    using Subsonic8.Settings;

    public class ShellViewModel : Screen, IShellViewModel
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        private IBottomBarViewModel _bottomBar;

        private IPlayerControls _playerControls;

        private Uri _source;

        #endregion

        #region Constructors and Destructors

        public ShellViewModel(
            IEventAggregator eventAggregator, 
            ISubsonicService subsonicService, 
            ICustomFrameAdapter navigationService, 
            IToastNotificationService notificationService, 
            IDialogNotificationService dialogNotificationService, 
            IStorageService storageService, 
            IWinRTWrappersService winRTWrappersService)
        {
            _eventAggregator = eventAggregator;
            SubsonicService = subsonicService;
            NavigationService = navigationService;
            NotificationService = notificationService;
            DialogNotificationService = dialogNotificationService;
            StorageService = storageService;
            WinRTWrappersService = winRTWrappersService;

            eventAggregator.Subscribe(this);
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

        public IDialogNotificationService DialogNotificationService { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Subsonic8";
            }
        }

        public Action<SearchResultCollection> NavigateToSearhResult { get; set; }

        public ICustomFrameAdapter NavigationService { get; set; }

        public IToastNotificationService NotificationService { get; set; }

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

        public IStorageService StorageService { get; set; }

        public ISubsonicService SubsonicService { get; set; }

        public IWinRTWrappersService WinRTWrappersService { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Handle(ChangeBottomBarMessage message)
        {
            BottomBar = message.BottomBarViewModel;
        }

        public async void HandleError(Exception error)
        {
            await new MessageDialogResult(error.ToString(), "Ooops...").Execute();
        }

        public void Pause()
        {
            _playerControls.PauseAction();
        }

        public void Play(PlaylistItem item, object options = null)
        {
            Source = item.Uri;
            _playerControls.PlayAction();
        }

        public void Resume()
        {
            _playerControls.PlayAction();
        }

        public void SendSearchQueryMessage(string query)
        {
            NavigationService.NavigateToViewModel<SearchViewModel>(query);
        }

        public void SongEnded()
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public void Stop()
        {
            _playerControls.StopAction();
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
        }

        #endregion
    }
}