namespace Client.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services.DataStructures.PlayerManagementService;

    public class PlayerManagementService : IPlayerManagementService
    {
        #region Static Fields

        private static readonly Dictionary<PlayerType, IPlayer> DefaultPlayers = new Dictionary<PlayerType, IPlayer>();

        private static readonly Dictionary<IPlayer, PlayerType> Players = new Dictionary<IPlayer, PlayerType>();

        #endregion

        #region Fields

        private readonly IEventAggregator _eventAggregator;

        private IPlayer _currnetPlayer;

        #endregion

        #region Constructors and Destructors

        public PlayerManagementService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region Public Events

        public event EventHandler<EventArgs> CurrentPlayerChanged;

        #endregion

        #region Public Properties

        public IPlayer AudioPlayer
        {
            get
            {
                return DefaultAudioPlayer ?? RegisteredAudioPlayers.FirstOrDefault();
            }
        }

        public IPlayer CurrentPlayer
        {
            get
            {
                return _currnetPlayer;
            }

            set
            {
                if (_currnetPlayer == value)
                {
                    return;
                }

                _currnetPlayer = value;
                RaiseCurrentPlayerChanged();
            }
        }

        public PlayerType CurrentPlayerType
        {
            get
            {
                return Players[CurrentPlayer];
            }
        }

        public IPlayer DefaultAudioPlayer
        {
            get
            {
                return GetDefaultPlayerByType(PlayerType.Audio);
            }

            set
            {
                DefaultPlayers[PlayerType.Audio] = value;
            }
        }

        public IVideoPlayer DefaultVideoPlayer
        {
            get
            {
                return GetDefaultPlayerByType(PlayerType.Video) as IVideoPlayer;
            }

            set
            {
                DefaultPlayers[PlayerType.Video] = value;
            }
        }

        public IEnumerable<IPlayer> RegisteredAudioPlayers
        {
            get
            {
                return GetPlayersByType(PlayerType.Audio);
            }
        }

        public IEnumerable<IPlayer> RegisteredPlayers
        {
            get
            {
                return Players.Keys;
            }
        }

        public IEnumerable<IPlayer> RegisteredVideoPlayers
        {
            get
            {
                return GetPlayersByType(PlayerType.Video);
            }
        }

        public IPlayer VideoPlayer
        {
            get
            {
                return DefaultVideoPlayer ?? RegisteredVideoPlayers.FirstOrDefault();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void ClearPlayers()
        {
            Players.Clear();
            DefaultPlayers.Clear();
        }

        public IPlayer GetPlayerFor(PlaylistItem item)
        {
            var player = item.Type == PlaylistItemTypeEnum.Audio ? AudioPlayer : VideoPlayer;

            return player;
        }

        public void Handle(StartPlaybackMessage message)
        {
            var player = GetPlayerFor(message.Item);
            CurrentPlayer = player;
            player.Play(message.Item, message.Options);
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

        public void RegisterAudioPlayer(IPlayer player)
        {
            Players.Add(player, PlayerType.Audio);
        }

        public void RegisterVideoPlayer(IPlayer player)
        {
            Players.Add(player, PlayerType.Video);
        }

        #endregion

        #region Methods

        private static IPlayer GetDefaultPlayerByType(PlayerType playerType)
        {
            return DefaultPlayers.Where(kvp => kvp.Key == playerType).Select(kvp => kvp.Value).SingleOrDefault();
        }

        private static IEnumerable<IPlayer> GetPlayersByType(PlayerType playerType)
        {
            return Players.Where(kvp => kvp.Value == playerType).Select(kvp => kvp.Key);
        }

        private void RaiseCurrentPlayerChanged()
        {
            if (CurrentPlayerChanged != null)
            {
                CurrentPlayerChanged(this, new EventArgs());
            }
        }

        #endregion
    }
}