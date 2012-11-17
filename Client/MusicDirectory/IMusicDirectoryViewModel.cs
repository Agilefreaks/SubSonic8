using Caliburn.Micro;
using Subsonic8.MenuItem;

namespace Subsonic8.MusicDirectory
{
    public interface IMusicDirectoryViewModel
    {
        Client.Common.Models.Subsonic.MusicDirectory Parameter { get; set; }

        BindableCollection<MenuItemViewModel> MenuItems { get; set; }
    }
}