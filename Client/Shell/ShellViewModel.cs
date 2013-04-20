using System;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Helpers;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.Services;
using Subsonic8.Search;
using Subsonic8.Settings;

namespace Subsonic8.Shell
{
    public class ShellViewModel : Screen, IShellViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private Uri _source;
        private IBottomBarViewModel _bottomBar;
        private IPlayerControls _playerControls;

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

        public IBottomBarViewModel BottomBar
        {
            get
            {
                return _bottomBar;
            }

            set
            {
                if (_bottomBar == value) return;
                _bottomBar = value;
                NotifyOfPropertyChange();
            }
        }

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

        public Action<SearchResultCollection> NavigateToSearhResult { get; set; }

        public ISubsonicService SubsonicService { get; set; }

        public ICustomFrameAdapter NavigationService { get; set; }

        public IToastNotificationService NotificationService { get; set; }

        public IDialogNotificationService DialogNotificationService { get; set; }

        public IStorageService StorageService { get; set; }

        public IWinRTWrappersService WinRTWrappersService { get; set; }

        public override string DisplayName
        {
            get { return "Subsonic8"; }
        }

        public ShellViewModel(IEventAggregator eventAggregator, ISubsonicService subsonicService, ICustomFrameAdapter navigationService,
            IToastNotificationService notificationService, IDialogNotificationService dialogNotificationService, 
            IStorageService storageService, IWinRTWrappersService winRTWrappersService)
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

        public void Handle(ChangeBottomBarMessage message)
        {
            BottomBar = message.BottomBarViewModel;
        }

        public void SendSearchQueryMessage(string query)
        {
            NavigationService.NavigateToViewModel<SearchViewModel>(query);
        }

        public void SongEnded()
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public async void HandleError(Exception error)
        {
            await new MessageDialogResult(error.ToString(), "Ooops...").Execute();
        }

        public void Play(Client.Common.Models.PlaylistItem item)
        {
            Source = item.Uri;
            _playerControls.PlayAction();
        }

        public void Pause()
        {
            _playerControls.PauseAction();
        }

        public void Resume()
        {
            _playerControls.PlayAction();
        }

        public void Stop()
        {
            _playerControls.StopAction();
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            WinRTWrappersService.RegisterSearchQueryHandler((sender, args) => SendSearchQueryMessage(args.QueryText));
            WinRTWrappersService.RegisterSettingsRequestedHandler((sender, args) => args.AddSetting<SettingsViewModel>());
            WinRTWrappersService.RegisterSettingsRequestedHandler((sender, args) => args.AddSetting<PrivacyPolicyViewModel>());
            WinRTWrappersService.RegisterMediaControlHandler(new MediaControlHandler(_eventAggregator));

            PlayerControls = (IPlayerControls)view;
        }
    }
}