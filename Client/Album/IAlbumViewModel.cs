using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.MenuItem;

namespace Subsonic8.Album
{
    public interface IAlbumViewModel
    {
        ISubsonicModel Parameter { get; set; }

        Client.Common.Models.Subsonic.Album Album { get; set; }

        BindableCollection<MenuItemViewModel> MenuItems { get; set; }
    }
}