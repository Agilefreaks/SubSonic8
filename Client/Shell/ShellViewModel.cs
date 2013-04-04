using System;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Messages;
using Subsonic8.Settings;
using Windows.UI.Xaml;

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
                if (_playerControls != null)
                {
                    HookupPlayerControls();
                }
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
            IToastNotificationService notificationService, IDialogNotificationService dialogNotificationService, IStorageService storageService, IWinRTWrappersService winRTWrappersService)
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

        public void PlayNext(object sender, RoutedEventArgs routedEventArgs)
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public void PlayPrevious(object sender, RoutedEventArgs routedEventArgs)
        {
            _eventAggregator.Publish(new PlayPreviousMessage());
        }

        public void PlayPause()
        {
            _eventAggregator.Publish(new PlayPauseMessage());
        }

        public void Stop()
        {
            _eventAggregator.Publish(new StopPlaybackMessage());
        }

        public void Handle(StartAudioPlaybackMessage message)
        {
            Source = message.Item.Uri;
        }

        public void Handle(StopAudioPlaybackMessage message)
        {
            if (_playerControls != null)
                _playerControls.Stop();
        }

        public void Handle(ResumePlaybackMessage message)
        {
            if (_playerControls != null)
                _playerControls.PlayPause();
        }

        public void Handle(PausePlaybackMessage message)
        {
            if (_playerControls != null)
                _playerControls.PlayPause();
        }

        public void Handle(ChangeBottomBarMessage message)
        {
            BottomBar = message.BottomBarViewModel;
        }

        public void SendSearchQueryMessage(string query)
        {
            _eventAggregator.Publish(new PerformSearch(query));
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            WinRTWrappersService.RegisterSearchQueryHandler((sender, args) => SendSearchQueryMessage(args.QueryText));
            WinRTWrappersService.RegisterSettingsRequestedHandler((sender, args) => args.AddSetting<SettingsViewModel>());

            PlayerControls = (IPlayerControls)view;
        }

        private void HookupPlayerControls()
        {
            _playerControls.PlayNextClicked += PlayNext;
            _playerControls.PlayPreviousClicked += PlayPrevious;
        }

        public async void HandleError(Exception error)
        {
            await new MessageDialogResult(error.ToString(), "Ooops...").Execute();
        }
    }
}