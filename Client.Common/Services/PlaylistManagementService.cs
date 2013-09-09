namespace Client.Common.Services
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services.DataStructures.PlaylistManagementService;
    using Action = System.Action;

    public class PlaylistManagementService : PropertyChangedBase, IPlaylistManagementService
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        private readonly Random _randomNumberGenerator;

        private bool _isPaused;

        private bool _isPlaying;

        private PlaylistItemCollection _items;

        private bool _shuffleOn;

        private bool _wasEmpty;

        #endregion

        #region Constructors and Destructors

        public PlaylistManagementService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            StartPlaybackAction = StartPlayback;
            StopPlaybackAction = StopPlayback;
            GetNextTrackNumberFunc = GetNextTrackNumber;
            GetPreviousTrackNumberFunc = GetPreviousTrackNumber;

            Items = new PlaylistItemCollection();
            PlaylistHistory = new PlaylistHistoryStack();
            _randomNumberGenerator = new Random();

            CurrentItem = null;
            _wasEmpty = true;
        }

        #endregion

        #region Public Properties

        public PlaylistItem CurrentItem { get; private set; }

        public int CurrentTrackNumber
        {
            get
            {
                return Items != null && CurrentItem != null && Items.Contains(CurrentItem)
                           ? Items.IndexOf(CurrentItem)
                           : -1;
            }
        }

        public Func<int> GetNextTrackNumberFunc { get; set; }

        public Func<int> GetPreviousTrackNumberFunc { get; set; }

        public bool HasElements
        {
            get
            {
                return Items.Any();
            }
        }

        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }

            private set
            {
                _isPaused = value;
                if (_isPaused)
                {
                    IsPlaying = false;
                }

                NotifyOfPropertyChange(() => IsPaused);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }

            private set
            {
                if (value.Equals(_isPlaying))
                {
                    return;
                }

                _isPlaying = value;
                if (_isPlaying)
                {
                    IsPaused = false;
                }

                NotifyOfPropertyChange();
            }
        }

        public PlaylistItemCollection Items
        {
            get
            {
                return _items;
            }

            private set
            {
                _items = value;
                if (_items != null)
                {
                    Items.CollectionChanged += PlaylistChanged;
                }
            }
        }

        public PlaylistHistoryStack PlaylistHistory { get; private set; }

        public bool ShuffleOn
        {
            get
            {
                return _shuffleOn;
            }

            private set
            {
                if (value.Equals(_shuffleOn))
                {
                    return;
                }

                _shuffleOn = value;
                NotifyOfPropertyChange();
            }
        }

        public Action<int> StartPlaybackAction { get; set; }

        public Action StopPlaybackAction { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Clear()
        {
            Items.Clear();
        }

        public int GetNextTrackNumber()
        {
            return ShuffleOn
                       ? _randomNumberGenerator.Next(Items.Count - 1)
                       : CurrentTrackNumber == (Items.Count - 1) ? 0 : CurrentTrackNumber + 1;
        }

        public int GetPreviousTrackNumber()
        {
            return ShuffleOn ? PlaylistHistory.Count == 0 ? -1 : PlaylistHistory.Pop() : (CurrentTrackNumber - 1);
        }

        public void Handle(PlayNextMessage message)
        {
            var previousTrackNumber = CurrentTrackNumber;
            StartPlaybackAction(GetNextTrackNumberFunc());
            if (previousTrackNumber != -1)
            {
                PlaylistHistory.Push(previousTrackNumber);
            }
        }

        public void Handle(PlayPreviousMessage message)
        {
            var previousTrackNumber = GetPreviousTrackNumberFunc();
            if (previousTrackNumber > -1)
            {
                StartPlaybackAction(previousTrackNumber);
            }
        }

        public void Handle(AddItemsMessage message)
        {
            if (message.Queue.Count == 0) return;
            Items.AddRange(message.Queue);
            if (message.StartPlaying)
            {
                StartPlaybackAction(Items.Count - message.Queue.Count);
            }
        }

        public void Handle(RemoveItemsMessage message)
        {
            foreach (var item in message.Queue)
            {
                Items.Remove(item);
            }
        }

        public void Handle(ToggleShuffleMessage message)
        {
            ShuffleOn = !ShuffleOn;
        }

        public void Handle(PlayPauseMessage message)
        {
            PlayPause();
        }

        public void Handle(PlayItemAtIndexMessage message)
        {
            var index = message.Index;
            if (index >= 0 && index < Items.Count)
            {
                StartPlaybackAction(index);
            }
        }

        public void Handle(StopMessage message)
        {
            StopPlaybackAction();
        }

        public void Handle(PauseMessage message)
        {
            Pause();
        }

        public void Handle(PlayMessage message)
        {
            Play(message.Options);
        }

        public void LoadPlaylist(PlaylistItemCollection playlistItemCollection)
        {
            if (playlistItemCollection == null)
            {
                return;
            }

            StopPlaybackAction();
            Items.Clear();
            Items.AddRange(playlistItemCollection);
        }

        public string GetStateAsString()
        {
            string result;
            var playlistServiceState = new PlaylistServiceState
                                           {
                                               CurrentTrackNumber = CurrentTrackNumber,
                                               IsShuffleOn = ShuffleOn,
                                               Items = Items,
                                               IsPlaying = IsPlaying
                                           };
            var serializer = GetStateSerializer();
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, playlistServiceState);
                memoryStream.Flush();
                result = Convert.ToBase64String(memoryStream.ToArray());
            }

            return result;
        }

        public void SetStateFromString(string stateString)
        {
            var bytes = Convert.FromBase64String(stateString);
            PlaylistServiceState state;
            using (var memoryStream = new MemoryStream(bytes))
            {
                try
                {
                    var serializer = GetStateSerializer();
                    state = (PlaylistServiceState)serializer.ReadObject(memoryStream);
                }
                catch (Exception)
                {
                    var serializer = GetLegacyStateSerializer();
                    state = (PlaylistServiceState)serializer.Deserialize(memoryStream);
                }
            }

            Items.Clear();
            Items.AddRange(state.Items);
            CurrentItem = state.CurrentTrackNumber > -1 ? Items.ElementAt(state.CurrentTrackNumber) : null;
            ShuffleOn = state.IsShuffleOn;
            if (state.IsPlaying)
            {
                Play();
            }
        }

        public void Pause()
        {
            if (!IsPlaying)
            {
                return;
            }

            _eventAggregator.Publish(new PausePlaybackMessage(CurrentItem));
            IsPaused = true;
        }

        public void Play(object options = null)
        {
            if (CurrentItem != null)
            {
                if (IsPaused)
                {
                    Resume();
                }
                else
                {
                    StartPlayback(options);
                }
            }
            else
            {
                StartPlayback(GetNextTrackNumber());
            }
        }

        public void PlayPause()
        {
            if (IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        public void Resume()
        {
            _eventAggregator.Publish(new ResumePlaybackMessage(CurrentItem));
            IsPlaying = true;
        }

        public void StartPlayback(int trackNumber)
        {
            StopPlaybackAction();
            CurrentItem = Items[trackNumber];
            StartPlayback();
        }

        public void StopPlayback()
        {
            if (CurrentItem == null)
            {
                return;
            }

            _eventAggregator.Publish(new StopPlaybackMessage(CurrentItem));
            CurrentItem.PlayingState = PlaylistItemState.NotPlaying;
            IsPlaying = false;
            IsPaused = false;
        }

        #endregion

        #region Methods

        private static XmlObjectSerializer GetStateSerializer()
        {
            return new DataContractSerializer(typeof(PlaylistServiceState), new[] { typeof(PlaylistItemCollection) });
        }

        private static XmlSerializer GetLegacyStateSerializer()
        {
            return new XmlSerializer(typeof(PlaylistServiceState), new[] { typeof(PlaylistItemCollection) });
        }

        private void PlaylistChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var totalElements = Items.Count;

            var becameNotEmpty = totalElements > 0 && _wasEmpty;
            var becameEmpty = totalElements == 0 && !_wasEmpty;

            if (becameEmpty)
            {
                StopPlayback();
                CurrentItem = null;
            }

            if (becameNotEmpty || becameEmpty)
            {
                _eventAggregator.Publish(new PlaylistStateChangedMessage(Items.Any()));
                _wasEmpty = !Items.Any();
            }
        }

        private void StartPlayback(object options = null)
        {
            _eventAggregator.Publish(new StartPlaybackMessage(CurrentItem) { Options = options });
            CurrentItem.PlayingState = PlaylistItemState.Playing;
            IsPlaying = true;
        }

        #endregion
    }
}