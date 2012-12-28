using System.Collections.ObjectModel;

namespace Subsonic8.BottomBar
{
    public interface IBottomBarViewModel
    {
        ObservableCollection<object> SelectedItems { get; set; }

        bool IsOpened { get; set; }

        bool IsPlaying { get; set; }

        void NavigateToPlaylist();

        void PlayPrevious();

        void PlayNext();

        void PlayPause();

        void Stop();
    }
}