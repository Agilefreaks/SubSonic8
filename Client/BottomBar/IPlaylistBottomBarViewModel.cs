using System;

namespace Subsonic8.BottomBar
{
    public interface IPlaylistBottomBarViewModel : IBottomBarViewModel
    {
        Action<int> DeletePlaylistAction { get; set; }
    }
}