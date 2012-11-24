using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.MenuItem;

namespace Subsonic8.MusicDirectory
{
    public interface IMusicDirectoryViewModel
    {
        ISubsonicModel Parameter { get; set; }

        Client.Common.Models.Subsonic.MusicDirectory MusicDirectory { get; set; }

        BindableCollection<MenuItemViewModel> MenuItems { get; set; }
    }
}