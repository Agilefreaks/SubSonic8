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
        private List<IGrouping<string, MenuItemViewModel>> _menuItems;

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
                return _menuItems;
            }

            private set
            {
                _menuItems = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        public SearchViewModel()
        {
            MenuItems = new List<IGrouping<string, MenuItemViewModel>>();
        }

        public void PopulateMenuItems()
        {
            if (Parameter == null) return;

            PopulateFrom(Parameter.Artists,
                         "Artists",
                         x => x.Name,
                         x => string.Format("{0} albums", x.AlbumCount));

            PopulateFrom(Parameter.Albums,
                        "Albums",
                        x => x.Name,
                        x => string.Format("{0} tracks", x.SongCount));

            PopulateFrom(Parameter.Songs,
                        "Songs",
                        x => x.Title,
                        x => string.Format("Artist: {0}, Album: {1}", x.Artist, x.Album));
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
        
        //SearchResultItemClick

        private void PopulateFrom<T>(IEnumerable<T> collection, string type, Func<T, string> title, Func<T, string> subtitle)
        {
            var result = collection.Select(x => new MenuItemViewModel
                                       {
                                           Type = type,
                                           Title = title(x),
                                           Item = x,
                                           Subtitle = subtitle(x)
                                       });

            var items = from i in result group i by type into g orderby g.Key select g;

            MenuItems.AddRange(items);
        }
    }
}