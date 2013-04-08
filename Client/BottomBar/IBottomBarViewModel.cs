using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;

namespace Subsonic8.BottomBar
{
    public interface IBottomBarViewModel : IHandle<PlaylistStateChangedMessage>
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