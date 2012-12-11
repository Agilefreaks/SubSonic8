using System.Collections.ObjectModel;

namespace Subsonic8.BottomBar
{
    public interface IDefaultBottomBarViewModel : IBottomBarViewModel
    {
        void AddToPlaylist();

        void RemoveFromPlaylist();

        ObservableCollection<object> SelectedItems { get; set; }
    }
}