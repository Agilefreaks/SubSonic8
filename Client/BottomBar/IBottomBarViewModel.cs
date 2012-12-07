namespace Subsonic8.BottomBar
{
    public interface IBottomBarViewModel
    {
        bool IsOpened { get; set; }

        bool IsPlaying { get; set; }

        void NavigateToPlaylist();

        void PlayPrevious();

        void PlayNext();

        void Play();

        void Stop();
    }
}