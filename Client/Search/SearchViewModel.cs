using System;
using System.Collections.Generic;
using System.Linq;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;

namespace Subsonic8.Search
{
    public class SearchViewModel : ViewModelBase, ISearchViewModel
    {
        private SearchResultCollection _parameter;
        private List<MenuItemViewModel> _menuItemViewModels;

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

        public SearchViewModel()
        {
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

        public void PopulateAlbums(List<Album> albums)
        {
            MenuItemViewModels.AddRange(albums.Select(a => a.AsMenuItemViewModel()));
        }

        public void PopulateSongs(List<Song> songs)
        {
            MenuItemViewModels.AddRange(songs.Select(a => a.AsMenuItemViewModel()));
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            SetCorrectState();
        }

        private void UpdateDisplayName()
        {
            DisplayName = string.Format("Searched for: \"{0}\"", Parameter.Query);
        }

        private void SetCorrectState()
        {
        }
    }
}