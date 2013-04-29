using System.Collections.ObjectModel;
using Caliburn.Micro;
using Subsonic8.BottomBar;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Framework.ViewModel
{
    public interface ICollectionViewModel<TParameter> : IViewModel
    {
        TParameter Parameter { get; set; }

        ObservableCollection<object> SelectedItems { get; }

        BindableCollection<MenuItemViewModel> MenuItems { get; set; }

        IDefaultBottomBarViewModel BottomBar { get; set; }

        void ChildClick(ItemClickEventArgs eventArgs);
    }
}