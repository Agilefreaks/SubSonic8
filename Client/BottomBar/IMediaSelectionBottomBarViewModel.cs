using System.Collections.ObjectModel;
using Subsonic8.MenuItem;

namespace Subsonic8.BottomBar
{
    public interface IMediaSelectionBottomBarViewModel : IBottomBarViewModel
    {
        void AddToPlaylist();

        void NavigateToPlaylist();

        ObservableCollection<MenuItemViewModel> SelectedItems { get; set; }
    }
}