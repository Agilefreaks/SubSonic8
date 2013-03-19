using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Messages;
using Subsonic8.Shell;

namespace Subsonic8.Playback
{
    public interface IPlaybackViewModel : IHandle<PlaylistMessage>, IHandle<PlaylistStateChangedMessage>,
        IHandle<PlayFile>, IHandle<StartVideoPlaybackMessage>, IHandle<StartAudioPlaybackMessage>, IHandle<StopVideoPlaybackMessage>, 
        IViewModel, IToastNotificationCapable
    {
        IShellViewModel ShellViewModel { get; set; }

        ISubsonicModel Parameter { get; set; }

        Uri Source { get; set; }

        PlaybackViewModelStateEnum State { get; }

        string CoverArt { get; set; }

        Func<IId, Task<Client.Common.Models.PlaylistItem>> LoadModel { get; set; }

        bool ShuffleOn { get; }

        bool IsPlaying { get; }

        TimeSpan EndTime { get; set; }

        void ClearPlaylist();

        void SavePlaylist();
    }
}