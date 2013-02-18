using System.ComponentModel;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;

namespace Client.Common.Services
{
    public interface IPlaylistManagementService : IHandle<PlayNextMessage>, IHandle<PlayPreviousMessage>, IHandle<AddItemsMessage>, IHandle<ToggleShuffleMessage>,
        IHandle<RemoveItemsMessage>, IHandle<PlayPauseMessage>, IHandle<PlayItemAtIndexMessage>, IHandle<StopPlaybackMessage>, INotifyPropertyChanged
    {
        PlaylistItem CurrentItem { get; }

        bool HasElements { get; }

        bool ShuffleOn { get; }

        bool IsPlaying { get; }

        PlaylistItemCollection Items { get; }

        void Clear();

        void LoadPlaylist(PlaylistItemCollection playlistItemCollection);
    }
}