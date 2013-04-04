using Caliburn.Micro;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;

namespace Subsonic8.Index
{
    public interface IIndexViewModel : IViewModel
    {
        string Parameter { get; set; }

        BindableCollection<MenuItemViewModel> MenuItems { get; set; }
    }
}