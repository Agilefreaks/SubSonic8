using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;

namespace Subsonic8.Search
{
    public class SearchResultsViewModel : ViewModelBase
    {
        private SearchResultCollection _parameter;

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
            }
        }

        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public SearchResultsViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            PopulateMenuItems();
            SetCorrectState();
        }

        private void UpdateDisplayName()
        {
            DisplayName = string.Format("Searched for: \"{0}\"", Parameter.Query);
        }

        private void SetCorrectState()
        {
        }

        private void PopulateMenuItems()
        {
            MenuItems.Clear();

            if (Parameter == null) return;

            foreach (var artist in Parameter.Artists)
            {
                MenuItems.Add(new MenuItemViewModel { Title = artist.Name, Item = artist, Subtitle = string.Format("{0} albums", artist.AlbumCount) });
            }

            foreach (var album in Parameter.Albums)
            {
                MenuItems.Add(new MenuItemViewModel { Title = album.Name, Item = album, Subtitle = string.Format("{0} tracks", album.SongCount) });
            }

            foreach (var song in Parameter.Songs)
            {
                MenuItems.Add(new MenuItemViewModel { Title = song.Title, Item = song, Subtitle = string.Format("Artist: {0}, Album: {1}", song.Artist, song.Album) });
            }
        }
    }
}