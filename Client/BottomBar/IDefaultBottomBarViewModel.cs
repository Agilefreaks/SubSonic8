using System;

namespace Subsonic8.BottomBar
{
    public interface IDefaultBottomBarViewModel : IBottomBarViewModel
    {
        Action NavigateOnPlay { get; set; }

        void AddToPlaylist();

        void PlayAll();

        bool CanAddToPlaylist { get; }
    }
}