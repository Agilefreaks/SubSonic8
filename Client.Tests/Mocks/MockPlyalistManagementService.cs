namespace Client.Tests.Mocks
{
    using System.Collections.Generic;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services;

    public class MockPlyalistManagementService : PropertyChangedBase, IPlaylistManagementService
    {
        #region Constructors and Destructors

        public MockPlyalistManagementService()
        {
            Items = new PlaylistItemCollection();
            MethodCalls = new Dictionary<string, object>();
        }

        #endregion

        #region Public Properties

        public int ClearCallCount { get; set; }

        public PlaylistItem CurrentItem { get; set; }

        public bool HasElements { get; set; }

        public bool IsPlaying { get; set; }

        public PlaylistItemCollection Items { get; set; }

        public int LoadPlaylistCallCount { get; set; }

        public Dictionary<string, object> MethodCalls { get; set; }

        public bool ShuffleOn { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Clear()
        {
            ClearCallCount++;
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

        public void LoadPlaylist(PlaylistItemCollection playlistItemCollection)
        {
            LoadPlaylistCallCount++;
            MethodCalls.Add("LoadPlaylist", playlistItemCollection);
        }

        #endregion
    }
}