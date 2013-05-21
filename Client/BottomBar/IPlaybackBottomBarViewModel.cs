namespace Subsonic8.BottomBar
{
    public interface IPlaybackBottomBarViewModel : IBottomBarViewModel
    {
        #region Public Properties

        bool CanRemoveFromPlaylist { get; }

        #endregion

        #region Public Methods and Operators

        void RemoveFromPlaylist();

        #endregion
    }
}