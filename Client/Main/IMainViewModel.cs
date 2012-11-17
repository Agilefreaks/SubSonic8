using System.Collections.Generic;
using Caliburn.Micro;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Main
{
    public interface IMainViewModel
    {
        BindableCollection<MenuItemViewModel> MenuItems { get; }

        void IndexClick(ItemClickEventArgs eventArgs);

        IEnumerable<IResult> Populate();
    }
}