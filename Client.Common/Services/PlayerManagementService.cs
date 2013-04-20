using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Services.DataStructures.PlayerManagementService;

namespace Client.Common.Services
{
    public class PlayerManagementService : IPlayerManagementService
    {
        private readonly IEventAggregator _eventAggregator;
        private static readonly Dictionary<IPlayer, PlayerType> Players = new Dictionary<IPlayer, PlayerType>();
        private static readonly Dictionary<PlayerType, IPlayer> DefaultPlayers = new Dictionary<PlayerType, IPlayer>();

        public IEnumerable<IPlayer> RegisteredPlayers
        {
            get { return Players.Keys; }
        }

        public IEnumerable<IPlayer> RegisteredAudioPlayers
        {
            get { return GetPlayersByType(PlayerType.Audio); }
        }

        public IEnumerable<IPlayer> RegisteredVideoPlayers
        {
            get { return GetPlayersByType(PlayerType.Video); }
        }

        public IPlayer DefaultVideoPlayer
        {
            get { return GetDefaultPlayerByType(PlayerType.Video); }
            set { DefaultPlayers[PlayerType.Video] = value; }
        }

        public IPlayer DefaultAudioPlayer
        {
            get { return GetDefaultPlayerByType(PlayerType.Audio); }
            set { DefaultPlayers[PlayerType.Audio] = value; }
        }

        public IPlayer VideoPlayer
        {
            get { return DefaultVideoPlayer ?? RegisteredVideoPlayers.FirstOrDefault(); }
        }

        public IPlayer AudioPlayer
        {
            get { return DefaultAudioPlayer ?? RegisteredAudioPlayers.FirstOrDefault(); }
        }

        public PlayerManagementService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public void RegisterVideoPlayer(IPlayer player)
        {
            Players.Add(player, PlayerType.Video);
        }

        public void RegisterAudioPlayer(IPlayer player)
        {
            Players.Add(player, PlayerType.Audio);
        }

        private static IEnumerable<IPlayer> GetPlayersByType(PlayerType playerType)
        {
            return Players.Where(kvp => kvp.Value == playerType).Select(kvp => kvp.Key);
        }

        private static IPlayer GetDefaultPlayerByType(PlayerType playerType)
        {
            return DefaultPlayers.Where(kvp => kvp.Key == playerType).Select(kvp => kvp.Value).SingleOrDefault();
        }

        public void Handle(StartPlaybackMessage message)
        {
            var player = GetPlayerFor(message.Item);
            player.Play(message.Item);
        }

        public void Handle(StopPlaybackMessage message)
        {
            var player = GetPlayerFor(message.Item);
            player.Stop();
        }

        public void Handle(PausePlaybackMessage message)
        {
            var player = GetPlayerFor(message.Item);
            player.Pause();
        }

        public void Handle(ResumePlaybackMessage message)
        {
            var player = GetPlayerFor(message.Item);
            player.Resume();
        }

        public IPlayer GetPlayerFor(PlaylistItem item)
        {
            var player = item.Type == PlaylistItemTypeEnum.Audio ? AudioPlayer : VideoPlayer;

            return player;
        }
    }
}