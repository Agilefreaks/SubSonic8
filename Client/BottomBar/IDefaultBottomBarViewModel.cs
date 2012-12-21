using System;

namespace Subsonic8.BottomBar
{
    public interface IDefaultBottomBarViewModel : IBottomBarViewModel
    {
        Action Navigate { get; set; }

        void AddToPlaylist();

        void PlayAll();

        void RemoveFromPlaylist();

        bool CanAddToPlaylist { get; }

        bool CanRemoveFromPlaylist { get; }
    }
}