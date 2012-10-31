using System.Diagnostics;
using Caliburn.Micro;
using Client.Common;
using Client.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Client.Menu
{
    public class MenuViewModel : ViewModelBase
    {
        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public MenuViewModel(INavigationService navigationService) : base(navigationService)
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            MenuItems.Add(new MenuItemViewModel { Title = "Title", Subtitle = "subtitle" });
        }

        public void Click(ItemClickEventArgs eventArgs)
        {
            Debugger.Break();
        }
    }
}