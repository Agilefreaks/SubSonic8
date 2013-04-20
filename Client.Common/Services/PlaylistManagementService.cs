using System;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Action = System.Action;

namespace Client.Common.Services
{
    public class PlaylistManagementService : PropertyChangedBase, IPlaylistManagementService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Random _randomNumberGenerator;
        private bool _shuffleOn;
        private bool _wasEmpty;
        private PlaylistItemCollection _items;
        private bool _isPlaying;

        public PlaylistHistoryStack PlaylistHistory { get; private set; }

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

        public void Clear()
        {
            Items.Clear();            
        }

        public void LoadPlaylist(PlaylistItemCollection playlistItemCollection)
        {            
            if (playlistItemCollection == null) return;
            StopPlaybackAction();
            Items.Clear();
            Items.AddRange(playlistItemCollection);
        }

        public PlaylistItem CurrentItem { get; private set; }

        public bool HasElements
        {
            get
            {
                return Items.Any();
            }
        }

        public bool ShuffleOn
        {
            get
            {
                return _shuffleOn;
            }

            private set
            {
                if (value.Equals(_shuffleOn)) return;
                _shuffleOn = value;
                NotifyOfPropertyChange();
            }
        }

        public Action<int> StartPlaybackAction { get; set; }

        public Action StopPlaybackAction { get; set; }

        public Func<int> GetNextTrackNumberFunc { get; set; }

        public Func<int> GetPreviousTrackNumberFunc { get; set; }

        public int CurrentTrackNumber
        {
            get
            {
                return Items != null && CurrentItem != null && Items.Contains(CurrentItem)
                           ? Items.IndexOf(CurrentItem)
                           : -1;
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
                if (value.Equals(_isPlaying)) return;
                _isPlaying = value;
                NotifyOfPropertyChange();
            }
        }

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
            Items.AddRange(message.Queue);
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
            if (CurrentItem != null)
            {
                if (!IsPlaying)
                {
                    Resume();
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
                Resume();
            }
        }

        public void Pause()
        {
            _eventAggregator.Publish(new PausePlaybackMessage(CurrentItem));
            IsPlaying = false;
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

        private void StartPlayback()
        {
            _eventAggregator.Publish(new StartPlaybackMessage(CurrentItem));
            CurrentItem.PlayingState = PlaylistItemState.Playing;
            IsPlaying = true;
        }

        public void StopPlayback()
        {
            if (CurrentItem == null) return;
            _eventAggregator.Publish(new StopPlaybackMessage(CurrentItem));
            CurrentItem.PlayingState = PlaylistItemState.NotPlaying;
            IsPlaying = false;
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

        private void PlaylistChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var totalElements = Items.Count;

            var becameNotEmpty = (totalElements > 0 && _wasEmpty);
            var becameEmpty = (totalElements == 0 && !_wasEmpty);

            if (becameNotEmpty || becameEmpty)
            {
                _eventAggregator.Publish(new PlaylistStateChangedMessage(Items.Any()));
                _wasEmpty = !Items.Any();
            }
        }
    }
}