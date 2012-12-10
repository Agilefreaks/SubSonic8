using System.Collections.ObjectModel;

namespace Subsonic8.BottomBar
{
    public interface IDefaultBottomBarViewModel : IBottomBarViewModel
    {
        void AddToPlaylist();

        ObservableCollection<object> SelectedItems { get; set; }
    }
}