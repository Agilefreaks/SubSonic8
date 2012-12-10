using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Messages;
using Subsonic8.PlaylistItem;

namespace Subsonic8.Playback
{
    public interface IPlaybackViewModel : IHandle<PlaylistMessage>, IHandle<PlayFile>, 
        IHandle<PlayNextMessage>, IHandle<PlayPreviousMessage>, 
        IHandle<PlayPauseMessage>, IHandle<StopMessage>,
        IViewModel
    {
        ISubsonicModel Parameter { get; set; }

        Uri Source { get; set; }

        PlaybackViewModelStateEnum State { get; set; }

        ObservableCollection<ISubsonicModel> Playlist { get; set; }

        ObservableCollection<PlaylistItemViewModel> PlaylistItems { get; set; }

        void StartPlayback();
    }
}