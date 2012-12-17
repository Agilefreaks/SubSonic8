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
        private SearchResultCollection _parameter;
        private List<MenuItemViewModel> _menuItemViewModels;
        private SearchResultState _state;

        public SearchResultCollection Parameter
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
                PopulateMenuItems();
                State = _menuItemViewModels.Any() ? SearchResultState.ResultsFound : SearchResultState.NoResultsFound;
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
            get { return _menuItemViewModels; }
            private set
            {
                _menuItemViewModels = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        public bool SearchSuccess
        {
            get { return _searchSuccess; }
         
            private set
            {
                _searchSuccess = value;
                NotifyOfPropertyChange();
            }
        }

        public SearchResultState State
        {
            get { return _state; }
            
            set
            {
                _state = value;
                NotifyOfPropertyChange();
            }
        }

        public SearchViewModel()
        {
            MenuItemViewModels = new List<MenuItemViewModel>();
            State = SearchResultState.NoResultsFound;
        }

        public void PopulateMenuItems()
        {
            if (Parameter == null) return;

            PopulateArtists(Parameter.Artists);
            PopulateAlbums(Parameter.Albums);
            PopulateSongs(Parameter.Songs);
        }

        public void PopulateArtists(List<ExpandedArtist> collection)
        {
            MenuItemViewModels.AddRange(collection.Select(a => a.AsMenuItemViewModel()));
        }

        public void PopulateAlbums(List<Client.Common.Models.Subsonic.Album> albums)
        {
            MenuItemViewModels.AddRange(albums.Select(a => a.AsMenuItemViewModel()));
        }

        public void PopulateSongs(List<Song> songs)
        {
            MenuItemViewModels.AddRange(songs.Select(a => a.AsMenuItemViewModel()));
        }

        public void SearchResultClick(ItemClickEventArgs eventArgs)
        {
            var navigableEntity = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateByEntityType(navigableEntity);
        }

        private void UpdateDisplayName()
        {
            DisplayName = string.Format("Searched for: \"{0}\"", Parameter.Query);
        }
    }
}