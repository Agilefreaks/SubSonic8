using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Search;
using Subsonic8.Settings;
using Windows.ApplicationModel.Search;
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
            }
        }

        public Action<SearchResultCollection> NavigateToSearhResult { get; set; }

        public ISubsonicService SubsonicService { get; set; }

        public INavigationService NavigationService { get; set; }

        public IToastNotificationService NotificationService { get; set; }

        public IDialogNotificationService DialogNotificationService { get; set; }

        public IStorageService StorageService { get; set; }

        public IWinRTWrappersService WinRTWrappersService { get; set; }

        public override string DisplayName
        {
            get { return "Subsonic8"; }
        }

        public ShellViewModel(IEventAggregator eventAggregator, ISubsonicService subsonicService, INavigationService navigationService,
            IToastNotificationService notificationService, IDialogNotificationService dialogNotificationService, IStorageService storageService, IWinRTWrappersService winRTWrappersService)
        {
            _eventAggregator = eventAggregator;
            SubsonicService = subsonicService;
            NavigationService = navigationService;
            NotificationService = notificationService;
            DialogNotificationService = dialogNotificationService;
            StorageService = storageService;
            WinRTWrappersService = winRTWrappersService;
            NavigateToSearhResult = NavigateToSearchResultCall;

            eventAggregator.Subscribe(this);
        }

        public async Task PerformSubsonicSearch(string query)
        {
            await SubsonicService.Search(query)
                               .WithErrorHandler(this)
                               .OnSuccess(result => NavigateToSearhResult(result))
                               .Execute();
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
                Source = null;
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

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            WinRTWrappersService.RegisterSearchQueryHandler(OnQuerySubmitted);
            WinRTWrappersService.RegisterSettingsRequestedHandler((sender, args) => args.AddSetting<SettingsViewModel>());

            HookupPlayerControls((IPlayerControls)view);

            LoadSettings();

            if (!SubsonicService.IsConfigured)
            {
                DialogNotificationService.Show(new DialogNotificationOptions
                                                   {
                                                       Message = ShellStrings.NotConfigured
                                                   });
                DialogService.ShowSettings<SettingsViewModel>();
            }
        }

        private void HookupPlayerControls(IPlayerControls playerControls)
        {
            _playerControls = playerControls;
            _playerControls.PlayNextClicked += PlayNext;
            _playerControls.PlayPreviousClicked += PlayPrevious;
        }

        private void NavigateToSearchResultCall(SearchResultCollection searchResultCollection)
        {
            NavigationService.NavigateToViewModel<SearchViewModel>(searchResultCollection);
        }

        private async void OnQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            await PerformSubsonicSearch(args.QueryText);
        }

        private async void LoadSettings()
        {
            var subsonic8Configuration = await GetSubsonic8Configuration();

            SubsonicService.Configuration = subsonic8Configuration.SubsonicServiceConfiguration;

            NotificationService.UseSound = subsonic8Configuration.ToastsUseSound;
        }

        private async Task<Subsonic8Configuration> GetSubsonic8Configuration()
        {
            var subsonic8Configuration = await StorageService.Load<Subsonic8Configuration>() ?? new Subsonic8Configuration();
#if DEBUG
            const string baseUrl = "http://192.168.0.121:4040/";
            subsonic8Configuration.SubsonicServiceConfiguration = new SubsonicServiceConfiguration
            {
                BaseUrl = baseUrl,
                Username = "admin",
                Password = "admin"
            };
#endif
            return subsonic8Configuration;
        }

        public async void HandleError(Exception error)
        {
            await new MessageDialogResult(error.ToString(), "Ooops...").Execute();
        }
    }
}