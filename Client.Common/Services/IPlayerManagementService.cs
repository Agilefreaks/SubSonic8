using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Services.DataStructures.PlayerManagementService;

namespace Client.Common.Services
{
    public interface IPlayerManagementService : IHandle<StartPlaybackMessage>, IHandle<StopPlaybackMessage>,
                                                IHandle<PausePlaybackMessage>, IHandle<ResumePlaybackMessage>
    {
        event EventHandler<EventArgs> CurrentPlayerChanged;
        IEnumerable<IPlayer> RegisteredPlayers { get; }
        IEnumerable<IPlayer> RegisteredAudioPlayers { get; }
        IEnumerable<IPlayer> RegisteredVideoPlayers { get; }
        IPlayer DefaultVideoPlayer { get; set; }
        IPlayer DefaultAudioPlayer { get; set; }
        IPlayer VideoPlayer { get; }
        IPlayer AudioPlayer { get; }
        IPlayer CurrentPlayer { get; set; }
        PlayerType CurrentPlayerType { get; }
        void RegisterVideoPlayer(IPlayer player);
        void RegisterAudioPlayer(IPlayer player);
        IPlayer GetPlayerFor(PlaylistItem item);
    }
}