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
        private int _currentTrackNo;
        private string _coverArt;

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

        public ObservableCollection<PlaylistItemViewModel> PlaylistItems
        {
            get { return _playlistItems; }
            set
            {
                _playlistItems = value;
                NotifyOfPropertyChange();
            }
        }

        public Action<PlaylistItemViewModel> Start { get; set; }

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

            UpdateDisplayName = () => DisplayName = "Playlist";
            Start = StartImpl;
            LoadModel = LoadModelImpl;

            // playlist stuff that need refactoring
            PlaylistItems = new ObservableCollection<PlaylistItemViewModel>();
            _currentTrackNo--;
        }

        public void StartPlayback(object e)
        {
            var pressedItem = (PlaylistItemViewModel)(((ItemClickEventArgs)e).ClickedItem);
            Start(pressedItem);
            _currentTrackNo = PlaylistItems.IndexOf(pressedItem);
        }

        public void StartImpl(PlaylistItemViewModel model)
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
                if (_currentTrackNo == -1)
                {
                    _currentTrackNo++;
                }

                Start(PlaylistItems[_currentTrackNo]);
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
            _currentTrackNo++;

            if (_currentTrackNo < PlaylistItems.Count)
            {
                Start(PlaylistItems[_currentTrackNo]);
            }
            else
            {
                StopAndReset();
            }
        }

        public void Previous()
        {
            _currentTrackNo--;
            if (_currentTrackNo > -1)
            {
                Start(PlaylistItems[_currentTrackNo]);
            }
            else
            {
                StopAndReset();
            }
        }

        public async void Handle(PlaylistMessage message)
        {
            if (message.ClearCurrent)
            {
                Stop();
                PlaylistItems.Clear();
            }

            foreach (var item in message.Queue)
            {
                await AddToPlaylist(item);
            }

            if (Source == null && ShellViewModel.Source == null && PlaylistItems.Any())
            {
                Start(PlaylistItems.First());
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
            Start(playlistItem);
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

        private async Task AddToPlaylist(ISubsonicModel item)
        {
            if (item.Type == SubsonicModelTypeEnum.Song || item.Type == SubsonicModelTypeEnum.Video)
            {
                PlaylistItems.Add(await LoadModel(item));
            }
            else
            {
                var children = new List<ISubsonicModel>();
                switch (item.Type)
                {
                    case SubsonicModelTypeEnum.Album:
                        {
                            var result = SubsonicService.GetAlbum(item.Id);
                            await result.Execute();
                            children.AddRange(result.Result.Songs);
                        } break;
                    case SubsonicModelTypeEnum.Artist:
                        {
                            var result = SubsonicService.GetArtist(item.Id);
                            await result.Execute();
                            children.AddRange(result.Result.Albums);
                        } break;
                    case SubsonicModelTypeEnum.MusicDirectory:
                        {
                            var result = SubsonicService.GetMusicDirectory(item.Id);
                            await result.Execute();
                            children.AddRange(result.Result.Children);
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

        private void StopAndReset()
        {
            _currentTrackNo = -1;
            Stop();
        }

        private async Task<PlaylistItemViewModel> LoadModelImpl(IId model)
        {
            PlaylistItemViewModel playlistItem = null;
            if (model != null)
            {
                var result = SubsonicService.GetSong(model.Id);
                await result.Execute();
                var item = result.Result;

                playlistItem = new PlaylistItemViewModel
                {
                    Artist = item.Artist,
                    Title = item.Title,
                    Item = item,
                    Uri = SubsonicService.GetUriForFileWithId(item.Id),
                    CoverArtId = item.CoverArt,
                    PlayingState = PlaylistItemState.NotPlaying,
                    Duration = item.Duration
                };
            }

            return playlistItem;
        }
    }
}