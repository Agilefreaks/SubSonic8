using System.Collections.Generic;
using System.Linq;
using Client.Common.Models.Subsonic;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Search
{
    public class SearchViewModel : ViewModelBase, ISearchViewModel
    {
        private string _parameter;
        private List<MenuItemViewModel> _menuItemViewModels;
        private SearchResultState _state;

        public string Parameter
        {
            get
            {
                return _parameter;
            }

            set
            {
                if (Equals(value, _parameter)) return;
                _parameter = value;
                NotifyOfPropertyChange();
                UpdateDisplayName();
                PerformSearch(Parameter);
            }
        }

        public List<IGrouping<string, MenuItemViewModel>> MenuItems
        {
            get
            {
                return (from item in MenuItemViewModels
                        group item by item.Type
                            into gr
                            orderby gr.Key
                            select gr).ToList();
            }
        }

        public List<MenuItemViewModel> MenuItemViewModels
        {
            get
            {
                return _menuItemViewModels;
            }

            private set
            {
                _menuItemViewModels = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        public SearchResultState State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
                NotifyOfPropertyChange();
            }
        }

        protected SearchResultCollection Results { get; set; }

        public SearchViewModel()
        {
            MenuItemViewModels = new List<MenuItemViewModel>();
            State = SearchResultState.NoResultsFound;
            UpdateDisplayName = () => DisplayName = string.Format("Searched for: \"{0}\"", Parameter ?? string.Empty);
        }

        public void PopulateMenuItems(SearchResultCollection result)
        {
            MenuItemViewModels.Clear();

            if (result == null) return;

            PopulateArtists(result.Artists);
            PopulateAlbums(result.Albums);
            PopulateSongs(result.Songs);

            foreach (var subsonicModel in _menuItemViewModels.Select(x => x.Item))
            {
                subsonicModel.CoverArt = SubsonicService.GetCoverArtForId(subsonicModel.CoverArt);
            }
        }

        public void PopulateArtists(List<ExpandedArtist> artists)
        {
            var menuItemViewModels = artists.Select(a => a.AsMenuItemViewModel()).ToList();
            RemoveCoverArt(menuItemViewModels);
            MenuItemViewModels.AddRange(menuItemViewModels);
        }

        public void PopulateAlbums(List<Client.Common.Models.Subsonic.Album> albums)
        {
            var menuItemViewModels = albums.Select(a => a.AsMenuItemViewModel()).ToList();
            RemoveCoverArt(menuItemViewModels);
            MenuItemViewModels.AddRange(menuItemViewModels);
        }

        public void PopulateSongs(List<Song> songs)
        {
            var menuItemViewModels = songs.Select(a => a.AsMenuItemViewModel()).ToList();
            MenuItemViewModels.AddRange(menuItemViewModels);
        }

        public void SearchResultClick(ItemClickEventArgs eventArgs)
        {
            var navigableEntity = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateByModelType(navigableEntity);
        }

        public async void PerformSearch(string parameter)
        {
            State = SearchResultState.Busy;
            await SubsonicService.Search(parameter)
                                 .WithErrorHandler(this)
                                 .OnSuccess(PopulateMenuItems)
                                 .Execute();

            State = MenuItemViewModels.Any() ? SearchResultState.ResultsFound : SearchResultState.NoResultsFound;
        }

        protected override void OnEventAggregatorSet()
        {
            base.OnEventAggregatorSet();
            EventAggregator.Subscribe(this);
        }

        private static void RemoveCoverArt(IEnumerable<MenuItemViewModel> menuItemViewModels)
        {
            foreach (var menuItemViewModel in menuItemViewModels)
            {
                menuItemViewModel.CoverArtId = string.Empty;
            }
        }
    }
}