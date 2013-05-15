using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockPlyalistManagementService : PropertyChangedBase, IPlaylistManagementService
    {
        public PlaylistItem CurrentItem { get; set; }

        public bool HasElements { get; set; }

        public bool ShuffleOn { get; set; }

        public bool IsPlaying { get; set; }

        public PlaylistItemCollection Items { get; set; }

        public int ClearCallCount { get; set; }

        public int LoadPlaylistCallCOunt { get; set; }

        public Dictionary<string, object> MethodCalls { get; set; }

        public MockPlyalistManagementService()
        {
            Items = new PlaylistItemCollection();
            MethodCalls = new Dictionary<string, object>();
        }

        public void Clear()
        {
            ClearCallCount++;
        }

        public void LoadPlaylist(PlaylistItemCollection playlistItemCollection)
        {
            LoadPlaylistCallCOunt++;
            MethodCalls.Add("LoadPlaylist", playlistItemCollection);
        }

        public void Handle(PlayNextMessage message)
        {
        }

        public void Handle(PlayPreviousMessage message)
        {
        }

        public void Handle(AddItemsMessage message)
        {
        }

        public void Handle(ToggleShuffleMessage message)
        {
        }

        public void Handle(RemoveItemsMessage message)
        {
        }

        public void Handle(PlayPauseMessage message)
        {
        }

        public void Handle(PlayItemAtIndexMessage message)
        {
        }

        public void Handle(StopMessage message)
        {
        }

        public void Handle(PauseMessage message)
        {
        }

        public void Handle(PlayMessage message)
        {
        }
    }
}