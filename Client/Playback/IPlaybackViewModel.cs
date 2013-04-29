using System;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Subsonic8.Framework.Behaviors;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.Playback
{
    public interface IPlaybackViewModel : IHandle<PlaylistStateChangedMessage>, IHandle<StartPlaybackMessage>,
        IViewModel, IToastNotificationCapable, IHaveState, IActiveItemProvider
    {
        int? Parameter { set; }

        Uri Source { get; set; }

        PlaybackViewModelStateEnum State { get; }

        string CoverArt { get; set; }

        bool IsPlaying { get; }

        void ClearPlaylist();

        void SavePlaylist();
    }
}