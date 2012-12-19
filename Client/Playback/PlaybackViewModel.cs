using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Subsonic8.Framework;
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
        private readonly INotificationManager _notificationManager;
        private IShellViewModel _shellViewModel;
        private ISubsonicModel _parameter;
        private PlaybackViewModelStateEnum _state;
        private Uri _source;
        private ObservableCollection<PlaylistItemViewModel> _playlistItems;
        private int _currentTrackNo;
        private bool _isPlaying;

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
                NotifyOfPropertyChange();
                StartPlayback();
            }
        }

        public Uri Source
        {
            get { return _source; }

            set
            {
                _source = value;
                NotifyOfPropertyChange();
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
                return _isPlaying;
            }
            
            set
            {
                _isPlaying = value; NotifyOfPropertyChange();
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

        #endregion

        public PlaybackViewModel(IEventAggregator eventAggregator, IShellViewModel shellViewModel, ISubsonicService subsonicService, INotificationManager notificationManager)
        {
            _eventAggregator = eventAggregator;
            ShellViewModel = shellViewModel;
            _notificationManager = notificationManager;
            _eventAggregator.Subscribe(this);
            SubsonicService = subsonicService;

            UpdateDisplayName = () => DisplayName = "Playlist";

            // playlist stuff that need refactoring
            PlaylistItems = new ObservableCollection<PlaylistItemViewModel>();
            _currentTrackNo--;
        }

        public void StartPlayback()
        {
            var song = Parameter;
            if (song != null)
            {
                if (song.Type == SubsonicModelTypeEnum.Song)
                {
                    Handle(new PlayFile { Id = song.Id });
                    State = PlaybackViewModelStateEnum.Audio;
                }
                else
                {
                    Source = SubsonicService.GetUriForFileWithId(song.Id);
                    ShellViewModel.Source = null;
                    State = PlaybackViewModelStateEnum.Video;
                }
            }
        }

        public void StartPlayback(object e)
        {
            var pressedItem = (PlaylistItemViewModel)(((ItemClickEventArgs)e).ClickedItem);
            ShellViewModel.Source = null;
            ShellViewModel.Source = pressedItem.Uri;
            _currentTrackNo = PlaylistItems.IndexOf(pressedItem);
        }

        public void Play()
        {
            if (PlaylistItems.Count > 0)
            {
                if (_currentTrackNo == -1)
                {
                    _currentTrackNo++;
                    PlayUri(PlaylistItems[_currentTrackNo].Uri);
                }
                else
                {
                    PlayUri(PlaylistItems[_currentTrackNo].Uri);
                }

                IsPlaying = true;
            }
        }

        public void Pause()
        {
            if (IsPlaying)
            {
                ShellViewModel.PlayPause();
                IsPlaying = false;
            }
        }

        public void Stop()
        {
            ShellViewModel.Stop();
        }

        public void Next()
        {
            _currentTrackNo++;

            if (_currentTrackNo < PlaylistItems.Count)
            {
                PlayUri(PlaylistItems[_currentTrackNo].Uri);
                _notificationManager.Show(new NotificationOptions
                {
                    ImageUrl = PlaylistItems[_currentTrackNo].CoverArt,
                    Title = PlaylistItems[_currentTrackNo].Title,
                    Subtitle = PlaylistItems[_currentTrackNo].Artist
                });
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
                PlayUri(PlaylistItems[_currentTrackNo].Uri);
            }
            else
            {
                StopAndReset();
            }
        }

        public void PlayUri(Uri source)
        {
            ShellViewModel.Source = source;
        }

        public void Handle(PlaylistMessage message)
        {
            foreach (var item in message.Queue)
            {
                var pi = new PlaylistItemViewModel();

                if ((item.Type == SubsonicModelTypeEnum.Song || item.Type == SubsonicModelTypeEnum.Video) && item is MusicDirectoryChild)
                {
                    var model = item as MusicDirectoryChild;
                    pi.Artist = model.Artist;
                    pi.CoverArt = model.CoverArt;
                    pi.Duration = model.Duration;
                    pi.Title = model.Title;
                    pi.Uri = item.Type == SubsonicModelTypeEnum.Song
                                 ? SubsonicService.GetUriForFileWithId(item.Id)
                                 : SubsonicService.GetUriForVideoWithId(item.Id);

                    PlaylistItems.Add(pi);
                }
            }
        }

        public void Handle(RemoveFromPlaylistMessage message)
        {
            foreach (var item in message.Queue)
            {
                PlaylistItems.Remove(item);
            }
        }

        public void Handle(PlayFile message)
        {
            Source = null;
            var source = SubsonicService.GetUriForFileWithId(message.Id);
            PlayUri(source);
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
            if (IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        public void Handle(StopMessage message)
        {
            Stop();
        }

        private void StopAndReset()
        {
            _currentTrackNo = -1;
            PlayUri(null);
        }
    }
}