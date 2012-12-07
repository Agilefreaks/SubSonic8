using System.Collections.ObjectModel;
using Subsonic8.MenuItem;

namespace Subsonic8.BottomBar
{
    public interface IDefaultBottomBarViewModel : IBottomBarViewModel
    {
        void AddToPlaylist();

        ObservableCollection<IMenuItemViewModel> SelectedItems { get; set; }
    }
}