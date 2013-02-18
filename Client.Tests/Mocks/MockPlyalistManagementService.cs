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

        public MockPlyalistManagementService()
        {
            Items = new PlaylistItemCollection();
        }

        public void Clear()
        {
            ClearCallCount++;            
        }

        public void Handle(PlayNextMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(PlayPreviousMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(AddItemsMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(ToggleShuffleMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(RemoveItemsMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(PlayPauseMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(PlayItemAtIndexMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(StopPlaybackMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}