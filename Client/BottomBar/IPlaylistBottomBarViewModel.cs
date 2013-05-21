namespace Subsonic8.BottomBar
{
    using System;
    using Client.Common.Results;

    public interface IPlaylistBottomBarViewModel : IBottomBarViewModel, IErrorHandler
    {
        #region Public Properties

        Action OnPlaylistDeleted { get; set; }

        #endregion
    }
}