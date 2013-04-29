using System;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.BottomBar
{
    public interface IDefaultBottomBarViewModel : IBottomBarViewModel, ISongLoader
    {
        Action NavigateOnPlay { get; set; }

        void AddToPlaylist();

        void PlayAll();

        bool CanAddToPlaylist { get; }

        IDialogNotificationService NotificationService { get; set; }
    }
}