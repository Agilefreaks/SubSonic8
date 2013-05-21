namespace Subsonic8.BottomBar
{
    using System;
    using Subsonic8.Framework.Services;
    using Subsonic8.Framework.ViewModel;

    public interface IDefaultBottomBarViewModel : IBottomBarViewModel, ISongLoader
    {
        #region Public Properties

        bool CanAddToPlaylist { get; }

        Action NavigateOnPlay { get; set; }

        IDialogNotificationService NotificationService { get; set; }

        #endregion

        #region Public Methods and Operators

        void AddToPlaylist();

        void PlayAll();

        #endregion
    }
}