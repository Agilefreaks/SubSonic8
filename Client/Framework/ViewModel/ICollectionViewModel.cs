namespace Subsonic8.Framework.ViewModel
{
    using System.Collections.ObjectModel;
    using Caliburn.Micro;
    using Subsonic8.BottomBar;
    using Subsonic8.MenuItem;
    using Windows.UI.Xaml.Controls;

    public interface ICollectionViewModel<TParameter> : IViewModel
    {
        #region Public Properties

        IBottomBarViewModel BottomBar { get; set; }

        BindableCollection<MenuItemViewModel> MenuItems { get; set; }

        TParameter Parameter { get; set; }

        ObservableCollection<object> SelectedItems { get; }

        #endregion

        #region Public Methods and Operators

        void ChildClick(ItemClickEventArgs eventArgs);

        #endregion
    }
}