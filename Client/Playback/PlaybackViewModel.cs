using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Services;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Messages;
using Subsonic8.PlaylistItem;
using Subsonic8.Shell;
using Windows.UI.Xaml.Controls;
using Action = System.Action;

namespace Subsonic8.Playback
{
    public class PlaybackViewModel : ViewModelBase, IPlaybackViewModel
    {
        #region Private Fields

        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationService _notificationService;
        private IShellViewModel _shellViewModel;
        private ISubsonicModel _parameter;
        private PlaybackViewModelStateEnum _state;
        private Uri _source;
        private ObservableCollection<PlaylistItemViewModel> _playlistItems;
        private int _currentTrackNumber;
        private string _coverArt;
        private bool _wasEmpty;
        private bool _shuffleOn;
        private readonly Random _randomNumberGenerator;
        private bool _playNextItem;

        #endregion

        #region Public Properties

        public IShellViewModel ShellViewModel
        {
            get
            {
                return _shellViewModel;
            }

            set
            {
                _shellViewModel = value; NotifyOfPropertyChange();
            }
        }

        public ISubsonicModel Parameter
        {
            get { return _parameter; }

            set
            {
                _parameter = value;
                if (_parameter != null)
                {
                    Handle(new PlayFile { Model = _parameter });
                }
            }
        }

        public Uri Source
        {
            get { return _source; }

            set
            {
                try
                {
                    _source = value;
                    NotifyOfPropertyChange();
                }
                catch (Exception exception)
                {
                    //This is due to a bug in winrt sdk
                    Debug.WriteLine(exception.ToString());
                }
            }
        }

        public PlaybackViewModelStateEnum State
        {
            get { return _state; }

            set
            {
                _state = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsPlaying
        {
            get
            {
                return PlaylistItems.Any(pi => pi.PlayingState == PlaylistItemState.Playing);
            }
        }

        public string CoverArt
        {
            get
            {
                return _coverArt;
            }

            set
            {
                _coverArt = value;
                NotifyOfPropertyChange();
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

        public ObservableCollection<PlaylistItemViewModel> PlaylistItems
        {
            get { return _playlistItems; }
            set
            {
                _playlistItems = value;
                NotifyOfPropertyChange();
            }
        }

        public PlaylistHistoryStack PlaylistHistory { get; private set; }

        public Action<PlaylistItemViewModel> StartAction { get; set; }

        public Action NextAction { get; set; }

        public Func<IId, Task<PlaylistItemViewModel>> LoadModel { get; set; }

        #endregion

        public PlaybackViewModel(IEventAggregator eventAggregator, IShellViewModel shellViewModel,
            ISubsonicService subsonicService, INotificationService notificationService)
        {
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;
            _eventAggregator.Subscribe(this);
            SubsonicService = subsonicService;
            ShellViewModel = shellViewModel;
            State = PlaybackViewModelStateEnum.Audio;
            _wasEmpty = true;

            UpdateDisplayName = () => DisplayName = "Playlist";
            StartAction = Start;
            NextAction = Next;
            LoadModel = LoadModelImpl;

            // playlist stuff that need refactoring
            _randomNumberGenerator = new Random();
            PlaylistHistory = new PlaylistHistoryStack();
            PlaylistItems = new ObservableCollection<PlaylistItemViewModel>();
            PlaylistItems.CollectionChanged += PlaylistChanged;
            _currentTrackNumber = -1;
        }

        public void StartPlayback(object e)
        {
            var pressedItem = (PlaylistItemViewModel)(((ItemClickEventArgs)e).ClickedItem);
            StartAction(pressedItem);
            _currentTrackNumber = PlaylistItems.IndexOf(pressedItem);
        }

        public void Start(PlaylistItemViewModel model)
        {
            Stop();
            if (model.Item.Type == SubsonicModelTypeEnum.Song)
            {
                Source = null;
                SetCoverArt(model.CoverArtId);
                SetPlaying(model);
                State = PlaybackViewModelStateEnum.Audio;
                PlayUri(model.Uri);
            }
            else
            {
                ShellViewModel.Source = null;
                State = PlaybackViewModelStateEnum.Video;
                SetPlaying(model);
                Source = SubsonicService.GetUriForVideoWithId(model.Item.Id);
            }

            _notificationService.Show(new NotificationOptions
                {
                    ImageUrl = SubsonicService.GetCoverArtForId(model.CoverArtId),
                    Title = model.Title,
                    Subtitle = model.Artist
                });
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

        public void Play()
        {
            if (PlaylistItems.Count > 0)
            {
                if (_currentTrackNumber == -1)
                {
                    _currentTrackNumber++;
                }

                StartAction(PlaylistItems[_currentTrackNumber]);
            }
        }

        public void Pause()
        {
            if (IsPlaying)
            {
                ShellViewModel.PlayPause();
                SetPlaying(null);
            }
        }

        public void Stop()
        {
            ShellViewModel.Stop();
            Source = null;
            SetPlaying(null);
        }

        public void Next()
        {
            var previousTrackNumber = _currentTrackNumber;
            _currentTrackNumber = GetNextTrackNumber();
            if (_currentTrackNumber < PlaylistItems.Count)
            {
                StartAction(PlaylistItems[_currentTrackNumber]);
                if (previousTrackNumber != -1)
                {
                    PlaylistHistory.Push(previousTrackNumber);
                }
            }
        }

        public void Previous()
        {
            _currentTrackNumber = GetPreviousTrackNumber();
            if (_currentTrackNumber > -1)
            {
                StartAction(PlaylistItems[_currentTrackNumber]);
            }
        }

        public async void Handle(PlaylistMessage message)
        {
            if (message.ClearCurrent)
            {
                Stop();
                PlaylistItems.Clear();
                _playNextItem = true;
            }

            foreach (var item in message.Queue)
            {
                await AddToPlaylist(item);
            }
        }

        public void Handle(RemoveFromPlaylistMessage message)
        {
            foreach (var item in message.Queue)
            {
                PlaylistItems.Remove(item);
            }
        }

        public async void Handle(PlayFile message)
        {
            var playlistItem = await LoadModel(message.Model);
            PlaylistItems.Add(playlistItem);
            StartAction(playlistItem);
        }

        public void Handle(PlayNextMessage message)
        {
            Next();
        }

        public void Handle(PlayPreviousMessage message)
        {
            Previous();
        }

        public void Handle(PlayPauseMessage message)
        {
            PlayPause();
        }

        public void Handle(StopMessage message)
        {
            Stop();
        }

        public void Handle(ToggleShuffleMessage message)
        {
            ShuffleOn = !ShuffleOn;
        }

        private async Task AddToPlaylist(ISubsonicModel item)
        {
            if (item.Type == SubsonicModelTypeEnum.Song || item.Type == SubsonicModelTypeEnum.Video)
            {
                PlaylistItems.Add(await LoadModel(item));
                if (_playNextItem)
                {
                    _playNextItem = false;
                    NextAction();
                }
            }
            else
            {
                var children = new List<ISubsonicModel>();
                switch (item.Type)
                {
                    case SubsonicModelTypeEnum.Album:
                        {
                            await SubsonicService.GetAlbum(item.Id)
                                                 .WithErrorHandler(this)
                                                 .OnSuccess(result => children.AddRange(result.Songs))
                                                 .Execute();

                        } break;
                    case SubsonicModelTypeEnum.Artist:
                        {
                            await SubsonicService.GetArtist(item.Id)
                                                 .WithErrorHandler(this)
                                                 .OnSuccess(result => children.AddRange(result.Albums))
                                                 .Execute();
                        } break;
                    case SubsonicModelTypeEnum.MusicDirectory:
                        {
                            await SubsonicService.GetMusicDirectory(item.Id)
                                                 .WithErrorHandler(this)
                                                 .OnSuccess(result => children.AddRange(result.Children))
                                                 .Execute();
                        } break;
                    case SubsonicModelTypeEnum.Index:
                        {
                            children.AddRange(((Client.Common.Models.Subsonic.IndexItem)item).Artists);
                        } break;
                }

                foreach (var subsonicModel in children)
                {
                    await AddToPlaylist(subsonicModel);
                }
            }
        }

        private bool ShouldPlayFirstSong()
        {
            return Source == null && ShellViewModel.Source == null && PlaylistItems.Any();
        }

        private void PlayUri(Uri source)
        {
            ShellViewModel.Source = source;
        }

        private void SetPlaying(PlaylistItemViewModel model)
        {
            foreach (var item in PlaylistItems.Where(pi => pi.PlayingState == PlaylistItemState.Playing))
            {
                item.PlayingState = PlaylistItemState.NotPlaying;
            }

            if (model != null)
            {
                model.PlayingState = PlaylistItemState.Playing;
            }

            BottomBar.IsPlaying = IsPlaying;
            NotifyOfPropertyChange(() => IsPlaying);
        }

        private void SetCoverArt(string coverArt)
        {
            CoverArt = SubsonicService.GetCoverArtForId(coverArt, ImageType.Original);
        }

        private void PlaylistChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var totalElements = _playlistItems.Count;

            var becameNotEmpty = (totalElements > 0 && _wasEmpty);
            var becameEmpty = (totalElements == 0 && !_wasEmpty);

            if (becameNotEmpty || becameEmpty)
            {
                var hasElements = _playlistItems.Any();
                var showControlsMessage = new ShowControlsMessage
                                              {
                                                  Show = hasElements
                                              };
                _eventAggregator.Publish(showControlsMessage);

                _wasEmpty = !hasElements;
            }
        }

        private async Task<PlaylistItemViewModel> LoadModelImpl(IId model)
        {
            PlaylistItemViewModel playlistItem = null;
            if (model != null)
            {
                await
                    SubsonicService.GetSong(model.Id)
                                   .WithErrorHandler(this)
                                   .OnSuccess(result => playlistItem = new PlaylistItemViewModel
                                       {
                                           Artist = result.Artist,
                                           Title = result.Title,
                                           Item = result,
                                           Uri = SubsonicService.GetUriForFileWithId(result.Id),
                                           CoverArtId = result.CoverArt,
                                           PlayingState = PlaylistItemState.NotPlaying,
                                           Duration = result.Duration
                                       }).Execute();
            }

            return playlistItem;
        }

        private int GetNextTrackNumber()
        {
            return ShuffleOn ? _randomNumberGenerator.Next(PlaylistItems.Count - 1) : _currentTrackNumber + 1;
        }

        private int GetPreviousTrackNumber()
        {
            return ShuffleOn ? PlaylistHistory.Count == 0 ? -1 : PlaylistHistory.Pop() : (_currentTrackNumber - 1);
        }
    }
}