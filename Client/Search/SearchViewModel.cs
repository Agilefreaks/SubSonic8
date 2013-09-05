namespace Subsonic8.Search
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.MenuItem;

    public class SearchViewModel : CollectionViewModelBase<string, SearchResultCollection>, ISearchViewModel
    {
        #region Fields

        private SearchResultState _state;

        #endregion

        #region Constructors and Destructors

        public SearchViewModel()
        {
            State = SearchResultState.NoResultsFound;
            UpdateDisplayName = () => DisplayName = string.Format("Searched for: \"{0}\"", Parameter ?? string.Empty);
        }

        #endregion

        #region Public Properties

        public List<IGrouping<string, MenuItemViewModel>> GroupedMenuItems
        {
            get
            {
                return (from item in MenuItems group item by item.Type into gr orderby gr.Key select gr).ToList();
            }
        }

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

        #endregion

        #region Public Methods and Operators

        public override async Task Populate()
        {
            State = SearchResultState.Busy;
            await base.Populate();
        }

        #endregion

        #region Methods

        protected override Task AfterPopulate(string parameter)
        {
            State = MenuItems.Any() ? SearchResultState.ResultsFound : SearchResultState.NoResultsFound;

            return Task.Factory.StartNew(() => { });
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

        protected override IServiceResultBase<SearchResultCollection> GetResult(string parameter)
        {
            return SubsonicService.Search(parameter);
        }

        private static void RemoveCoverArt(IEnumerable<IMediaModel> menuItemViewModels)
        {
            foreach (var menuItemViewModel in menuItemViewModels)
            {
                menuItemViewModel.CoverArt = null;
            }
        }

        #endregion
    }
}