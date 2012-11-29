using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.Messages;

namespace Subsonic8.Playback
{
    public interface IPlaybackViewModel : IHandle<PlaylistMessage>, IHandle<PlayFile>
    {
        ISubsonicModel Parameter { get; set; }
        Uri Source { get; set; }
        PlaybackViewModelStateEnum State { get; set; }
        ObservableCollection<ISubsonicModel> Playlist { get; set; }
        void StartPlayback();
    }
}