namespace Client.Common.Services
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services.DataStructures.PlayerManagementService;

    public interface IPlayerManagementService : IHandle<StartPlaybackMessage>, 
                                                IHandle<StopPlaybackMessage>, 
                                                IHandle<PausePlaybackMessage>, 
                                                IHandle<ResumePlaybackMessage>
    {
        #region Public Events

        event EventHandler<EventArgs> CurrentPlayerChanged;

        #endregion

        #region Public Properties

        IPlayer AudioPlayer { get; }

        IPlayer CurrentPlayer { get; set; }

        PlayerType CurrentPlayerType { get; }

        IPlayer DefaultAudioPlayer { get; set; }

        IVideoPlayer DefaultVideoPlayer { get; set; }

        IEnumerable<IPlayer> RegisteredAudioPlayers { get; }

        IEnumerable<IPlayer> RegisteredPlayers { get; }

        IEnumerable<IPlayer> RegisteredVideoPlayers { get; }

        IPlayer VideoPlayer { get; }

        #endregion

        #region Public Methods and Operators

        IPlayer GetPlayerFor(PlaylistItem item);

        void RegisterAudioPlayer(IPlayer player);

        void RegisterVideoPlayer(IPlayer player);

        #endregion
    }
}