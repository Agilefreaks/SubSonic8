namespace Subsonic8.Framework.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.Models;
    using Subsonic8.BottomBar;
    using Subsonic8.MenuItem;

    public interface ICollectionViewModel<TParameter> : IViewModel
    {
        #region Public Properties

        IBottomBarViewModel BottomBar { get; set; }

        BindableCollection<MenuItemViewModel> MenuItems { get; set; }

        TParameter Parameter { get; set; }

        ObservableCollection<object> SelectedItems { get; }

        Func<IId, Task<PlaylistItem>> LoadPlaylistItem { get; set; }

        #endregion

        #region Public Methods and Operators

        Task HandleItemSelection(ISubsonicModel subsonicModel);

        #endregion
    }
}