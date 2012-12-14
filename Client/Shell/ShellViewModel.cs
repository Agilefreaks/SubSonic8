using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Subsonic8.BottomBar;
using Subsonic8.Messages;
using Subsonic8.Search;
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
                if (Equals(value, _source)) return;
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

        public ISubsonicService SubsonicService { get; set; }

        public IPlayerControls PlayerControls
        {
            get { return _playerControls; }
            set
            {
                _playerControls = value;
                NotifyOfPropertyChange();
            }
        }

        public INavigationService NavigationService { get; set; }

        public Action<SearchResultCollection> NavigateToSearhResult { get; set; }

        public ShellViewModel(IEventAggregator eventAggregator, ISubsonicService subsonicService, INavigationService navigationService)
        {
            _eventAggregator = eventAggregator;
            SubsonicService = subsonicService;
            NavigationService = navigationService;
            NavigateToSearhResult = NavigateToSearchResultCall;
            eventAggregator.Subscribe(this);
        }

        public async Task PerformSubsonicSearch(string query)
        {
            var searchResult = SubsonicService.Search(query);
            await searchResult.Execute();

            NavigateToSearhResult(searchResult.Result);
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

            SearchPane.GetForCurrentView().QuerySubmitted += OnQuerySubmitted;

            if ((_playerControls = view as IPlayerControls) != null)
            {
                _playerControls.PlayNextClicked += PlayNext;
                _playerControls.PlayPreviousClicked += PlayPrevious;
            }
        }

        private void NavigateToSearchResultCall(SearchResultCollection searchResultCollection)
        {
            NavigationService.NavigateToViewModel<SearchViewModel>(searchResultCollection);
        }

        private async void OnQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            await PerformSubsonicSearch(args.QueryText);
        }

        public void MainAppBarClosed(object sender, object e)
        {
            Debugger.Break();
            BottomBar.IsOpened = BottomBar.SelectedItems.Any();
        }
    }
}