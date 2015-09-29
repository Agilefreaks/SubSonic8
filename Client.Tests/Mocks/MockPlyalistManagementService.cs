namespace Client.Tests.Mocks
{
    using System;
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
            SetStateFromStringCalls = new List<string>();
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

        public List<string> SetStateFromStringCalls { get; set; }

        public Func<string> GetStateAsStringCallback { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Clear()
        {
            ClearCallCount++;
        }

        public void Handle(JumpToNextMessage message)
        {
        }

        public void Handle(JumpToPreviousMessage message)
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

        public void Handle(ToggleRepeatMessage message)
        {
        }

        public void Handle(PlayNextMessage message)
        {
        }

        public void LoadPlaylist(PlaylistItemCollection playlistItemCollection)
        {
            LoadPlaylistCallCount++;
            MethodCalls.Add("LoadPlaylist", playlistItemCollection);
        }

        public string GetStateAsString()
        {
            return GetStateAsStringCallback != null ? GetStateAsStringCallback() : string.Empty;
        }

        public void SetStateFromString(string stateString)
        {
            SetStateFromStringCalls.Add(stateString);
        }

        #endregion
    }
}