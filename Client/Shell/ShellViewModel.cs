using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.Messages;
using Subsonic8.Search;
using Windows.ApplicationModel.Search;

namespace Subsonic8.Shell
{
    public class ShellViewModel : Screen, IShellViewModel
    {
        private Uri _source;

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

        public ISubsonicService SubsonicService { get; set; }

        public INavigationService NavigationService { get; set; }

        public Action<SearchResultCollection> NavigateToSearhResult { get; set; }

        public ShellViewModel(IEventAggregator eventAggregator, ISubsonicService subsonicService, INavigationService navigationService)
        {
            SubsonicService = subsonicService;
            NavigationService = navigationService;
            NavigateToSearhResult = NavigateToSearchResultCall;
            eventAggregator.Subscribe(this);
        }

        public void Handle(PlayFile message)
        {
            Source = SubsonicService.GetUriForFileWithId(message.Id);
        }

        public async Task PerformSubsonicSearch(string query)
        {
            var searchResult = SubsonicService.Search(query);
            await searchResult.Execute();

            NavigateToSearhResult(searchResult.Result);
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            SearchPane.GetForCurrentView().QuerySubmitted += OnQuerySubmitted;
        }

        private void NavigateToSearchResultCall(SearchResultCollection searchResultCollection)
        {
            NavigationService.NavigateToViewModel<SearchViewModel>(searchResultCollection);            
        }

        private async void OnQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            await PerformSubsonicSearch(args.QueryText);
        }
    }
}