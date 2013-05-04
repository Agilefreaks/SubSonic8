using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;

namespace Subsonic8.Search
{
    public class SearchViewModel : CollectionViewModelBase<string, SearchResultCollection>, ISearchViewModel
    {
        private SearchResultState _state;

        public SearchResultState State
        {
            get
            {
                return _state;
            }

            private set
            {
                _state = value;
                NotifyOfPropertyChange();
            }
        }

        public List<IGrouping<string, MenuItemViewModel>> GroupedMenuItems
        {
            get
            {
                return (from item in MenuItems
                        group item by item.Type
                            into gr
                            orderby gr.Key
                            select gr).ToList();
            }
        }

        public SearchViewModel()
        {
            State = SearchResultState.NoResultsFound;
            UpdateDisplayName = () => DisplayName = string.Format("Searched for: \"{0}\"", Parameter ?? string.Empty);
        }

        protected override void Populate()
        {
            State = SearchResultState.Busy;
            base.Populate();
        }

        protected override IServiceResultBase<SearchResultCollection> GetResult(string parameter)
        {
            return SubsonicService.Search(parameter);
        }

        protected override IEnumerable<IMediaModel> GetItemsToDisplay(SearchResultCollection result)
        {
            var models = new List<IMediaModel>();
            if (result != null)
            {
                RemoveCoverArt(result.Artists);
                RemoveCoverArt(result.Albums);

                models.AddRange(result.Artists);
                models.AddRange(result.Albums);
                models.AddRange(result.Songs);
            }

            return models;
        }

        protected override Task AfterPopulate(string parameter)
        {
            State = MenuItems.Any() ? SearchResultState.ResultsFound : SearchResultState.NoResultsFound;

            return Task.Factory.StartNew(() => { });
        }

        private static void RemoveCoverArt(IEnumerable<IMediaModel> menuItemViewModels)
        {
            foreach (var menuItemViewModel in menuItemViewModels)
            {
                menuItemViewModel.CoverArt = null;
            }
        }
    }
}