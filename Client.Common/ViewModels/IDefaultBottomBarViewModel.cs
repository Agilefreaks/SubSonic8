using System.Collections.ObjectModel;

namespace Client.Common.ViewModels
{
    public interface IDefaultBottomBarViewModel : IBottomBarViewModel
    {
        void AddToPlaylist();

        ObservableCollection<IMenuItemViewModel> SelectedItems { get; set; }
    }
}