using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Messages;
using Subsonic8.PlaylistItem;
using Subsonic8.Shell;
using Action = System.Action;

namespace Subsonic8.Playback
{
    public interface IPlaybackViewModel : IHandle<PlaylistMessage>, IHandle<RemoveFromPlaylistMessage>,
        IHandle<PlayFile>, IHandle<PlayNextMessage>, IHandle<PlayPreviousMessage>,
        IHandle<PlayPauseMessage>, IHandle<StopMessage>, IHandle<ToggleShuffleMessage>,
        IViewModel
    {
        IShellViewModel ShellViewModel { get; set; }

        ISubsonicModel Parameter { get; set; }

        Uri Source { get; set; }

        PlaybackViewModelStateEnum State { get; set; }

        bool IsPlaying { get; }

        string CoverArt { get; set; }

        ObservableCollection<PlaylistItemViewModel> PlaylistItems { get; set; }

        Action<PlaylistItemViewModel> StartAction { get; set; }

        Action NextAction { get; set; }

        Func<IId, Task<PlaylistItemViewModel>> LoadModel { get; set; }

        bool ShuffleOn { get; }

        PlaylistHistoryStack PlaylistHistory { get; }

        void Play();

        void Pause();

        void Stop();

        void Next();

        void Previous();

        void PlayPause();
    }
}