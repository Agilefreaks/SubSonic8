using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Client.Common.Models.Subsonic;
using Client.Common.ViewModels;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Subsonic8.Shell;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Search
{
    public class SearchViewModel : ViewModelBase, ISearchViewModel
    {
        private readonly IShellViewModel _shellViewModel;
        private readonly IMediaSelectionBottomBarViewModel _mediaSelectionBottomBarViewModel;
        private SearchResultCollection _parameter;
        private List<MenuItemViewModel> _menuItemViewModels;

        public ObservableCollection<MenuItemViewModel> SelectedItems
        {
            get { return _mediaSelectionBottomBarViewModel.SelectedItems; }
        }

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

        public SearchViewModel(IShellViewModel shellViewModel, IMediaSelectionBottomBarViewModel mediaSelectionBottomBarViewModel)
        {
            _shellViewModel = shellViewModel;
            _mediaSelectionBottomBarViewModel = mediaSelectionBottomBarViewModel;

            MenuItemViewModels = new List<MenuItemViewModel>();
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
            var navigableEntity = ((MenuItemViewModel) eventArgs.ClickedItem).Item;

            NavigationService.NavigateByEntityType(navigableEntity);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _shellViewModel.BottomBar = _mediaSelectionBottomBarViewModel;
        }

        private void UpdateDisplayName()
        {
            DisplayName = string.Format("Searched for: \"{0}\"", Parameter.Query);
        }
    }
}