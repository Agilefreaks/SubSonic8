namespace Subsonic8.BottomBar
{
    public interface IBottomBarViewModel
    {
        bool IsOpened { get; set; }

        void NavigateToPlaylist();

        void PlayPrevious();

        void PlayNext();

        void PlayPause();

        void Stop();
    }
}