namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services;
    using Client.Common.Services.DataStructures.PlayerManagementService;

    public class MockPlayerManagementService : IPlayerManagementService
    {
        public event EventHandler<EventArgs> CurrentPlayerChanged;

        public IPlayer AudioPlayer { get; private set; }

        public IPlayer CurrentPlayer { get; set; }

        public PlayerType CurrentPlayerType { get; private set; }

        public IPlayer DefaultAudioPlayer { get; set; }

        public IVideoPlayer DefaultVideoPlayer { get; set; }

        public IEnumerable<IPlayer> RegisteredAudioPlayers { get; private set; }

        public IEnumerable<IPlayer> RegisteredPlayers { get; private set; }

        public IEnumerable<IPlayer> RegisteredVideoPlayers { get; private set; }

        public IPlayer VideoPlayer { get; private set; }

        public void Handle(StartPlaybackMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StopPlaybackMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(PausePlaybackMessage message)
        {
            throw new NotImplementedException();
        }

        public void Handle(ResumePlaybackMessage message)
        {
            throw new NotImplementedException();
        }

        public IPlayer GetPlayerFor(PlaylistItem item)
        {
            throw new NotImplementedException();
        }

        public void RegisterAudioPlayer(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void RegisterVideoPlayer(IPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}