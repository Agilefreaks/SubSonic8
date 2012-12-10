using Caliburn.Micro;
using Client.Common.Results;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Main
{
    public interface IMainViewModel : IViewModel
    {
        BindableCollection<MenuItemViewModel> MenuItems { get; }

        void IndexClick(ItemClickEventArgs eventArgs);

        void Populate();
        void SetMenuItems(IGetRootResult getIndexResult);
    }
}