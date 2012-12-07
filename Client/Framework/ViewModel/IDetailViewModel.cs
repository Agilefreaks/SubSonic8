using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.MenuItem;

namespace Subsonic8.Framework.ViewModel
{
    public interface IDetailViewModel<T> : IViewModel
        where T : ISubsonicModel
    {
        ISubsonicModel Parameter { get; set; }

        T Item { get; set; }

        BindableCollection<MenuItemViewModel> MenuItems { get; set; }
    }
}