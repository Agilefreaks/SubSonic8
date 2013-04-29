using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Subsonic8.Framework.Behaviors;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Messages;

namespace Subsonic8.Playback
{
    public interface IPlaybackViewModel : IHandle<PlaylistMessage>, IHandle<PlaylistStateChangedMessage>,
        IHandle<PlayFile>, IHandle<StartPlaybackMessage>,
        IViewModel, IToastNotificationCapable, IHaveState, IActiveItemProvider
    {
        int? Parameter { set; }

        Uri Source { get; set; }

        PlaybackViewModelStateEnum State { get; }

        string CoverArt { get; set; }

        Func<IId, Task<Client.Common.Models.PlaylistItem>> LoadModel { get; set; }

        bool IsPlaying { get; }

        void ClearPlaylist();

        void SavePlaylist();
    }
}