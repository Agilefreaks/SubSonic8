using Subsonic8.BottomBar;

namespace Subsonic8.Shell
{
    public interface IMenuBarViewModelProvider
    {
        IPlaylistBarViewModel BottomBar { get; set; }
    }
}
