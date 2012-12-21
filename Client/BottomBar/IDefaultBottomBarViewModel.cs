namespace Subsonic8.BottomBar
{
    public interface IDefaultBottomBarViewModel : IBottomBarViewModel
    {
        void AddToPlaylist();

        void PlayAll();

        void RemoveFromPlaylist();

        bool CanAddToPlaylist { get; }

        bool CanRemoveFromPlaylist { get; }
    }
}