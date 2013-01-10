using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Messages;
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

        public INotificationService NotificationService { get; set; }

        public IStorageService StorageService { get; set; }

        public IWinRTWrappersService WinRTWrappersService { get; set; }

        public ShellViewModel(IEventAggregator eventAggregator, ISubsonicService subsonicService, INavigationService navigationService,
            INotificationService notificationService, IStorageService storageService, IWinRTWrappersService winRTWrappersService)
        {
            _eventAggregator = eventAggregator;
            SubsonicService = subsonicService;
            NavigationService = navigationService;
            NotificationService = notificationService;
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
            if (_playerControls != null)
                _playerControls.PlayPause();
        }

        public void Stop()
        {
            if (_playerControls != null)
                Source = null;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            WinRTWrappersService.RegisterSearchQueryHandler(OnQuerySubmitted);
            WinRTWrappersService.RegisterSettingsRequestedHandler((sender, args) => args.AddSetting<SettingsViewModel>());

            HookupPlayerControls((IPlayerControls)view);

            LoadSettings();
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
            var subsonic8Configuration = await StorageService.Load<Subsonic8Configuration>() ??new Subsonic8Configuration();
#if DEBUG
            const string baseUrl = "http://cristibadila.dynalias.com:33770/music/";
            subsonic8Configuration.SubsonicServiceConfiguration = new SubsonicServiceConfiguration
            {
                BaseUrl = baseUrl,
                Username = "media",
                Password = "media"
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