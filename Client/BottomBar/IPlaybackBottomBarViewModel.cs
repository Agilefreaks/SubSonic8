namespace Subsonic8.BottomBar
{
    public interface IPlaybackBottomBarViewModel : IBottomBarViewModel
    {
        bool CanRemoveFromPlaylist { get; }

        void RemoveFromPlaylist();
    }
}