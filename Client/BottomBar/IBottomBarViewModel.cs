using System.Collections.ObjectModel;
using Caliburn.Micro;
using Subsonic8.Messages;

namespace Subsonic8.BottomBar
{
    public interface IBottomBarViewModel : IHandle<ShowControlsMessage>
    {
        ObservableCollection<object> SelectedItems { get; set; }

        bool IsOpened { get; set; }

        bool IsPlaying { get; set; }

        bool IsOnPlaylist { get; set; }

        bool DisplayPlayControls { get; set; }

        void NavigateToPlaylist();

        void PlayPrevious();

        void PlayNext();

        void PlayPause();

        void Stop();
    }
}