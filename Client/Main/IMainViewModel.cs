namespace Subsonic8.Main
{
    using System.Collections.Generic;
    using Caliburn.Micro;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.MenuItem;
    using Windows.UI.Xaml.Controls;

    public interface IMainViewModel : IViewModel, IResultHandler<IList<MusicFolder>>
    {
        #region Public Properties

        BindableCollection<MenuItemViewModel> MenuItems { get; }

        #endregion

        #region Public Methods and Operators

        void IndexClick(ItemClickEventArgs eventArgs);

        void Populate();

        void SetMenuItems(IList<MusicFolder> items);

        #endregion
    }
}