using System.Collections.ObjectModel;
using Subsonic8.MenuItem;

namespace Subsonic8.BottomBar
{
    public interface IPlaylistBarViewModel
    {
        ObservableCollection<MenuItemViewModel> SelectedItems { get; set; }

        bool IsOpened { get; set; }

        void AddToPlaylist();
    }
}