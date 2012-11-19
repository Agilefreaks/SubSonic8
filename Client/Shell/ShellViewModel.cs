using System;
using Caliburn.Micro;
using Client.Common;
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

        public ShellViewModel(IEventAggregator eventAggregator, ISubsonicService subsonicService, INavigationService navigationService)
        {
            SubsonicService = subsonicService;
            NavigationService = navigationService;
            eventAggregator.Subscribe(this);
        }

        public void Handle(PlayFile message)
        {
            Source = SubsonicService.GetUriForFileWithId(message.Id);
        }

        public async void PerformSubsonicSearch(string query)
        {
            var searchResult = SubsonicService.Search(query);
            await searchResult.Execute();

            NavigationService.NavigateToViewModel<SearchResultsViewModel>(searchResult.Result);
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            SearchPane.GetForCurrentView().QuerySubmitted += OnQuerySubmitted;
        }

        private void OnQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            PerformSubsonicSearch(args.QueryText);
        }
    }
}