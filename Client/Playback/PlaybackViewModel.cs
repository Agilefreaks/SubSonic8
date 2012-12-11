using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
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
        private readonly IShellViewModel _shellViewModel;
        private ISubsonicModel _parameter;
        private PlaybackViewModelStateEnum _state;
        private Uri _source;
        private ObservableCollection<ISubsonicModel> _playlist;
        private ObservableCollection<PlaylistItemViewModel> _playlistItems;
        private int _currentTrackNo;
        #endregion

        #region Public Properties

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

        public ObservableCollection<ISubsonicModel> Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;
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

        #endregion

        public PlaybackViewModel(IEventAggregator eventAggregator, IShellViewModel shellViewModel, ISubsonicService subsonicService)
        {
            _eventAggregator = eventAggregator;
            _shellViewModel = shellViewModel;
            _eventAggregator.Subscribe(this);
            SubsonicService = subsonicService;
            Playlist = new ObservableCollection<ISubsonicModel>();
            PlaylistItems = new ObservableCollection<PlaylistItemViewModel>();
        }

        public void StartPlayback()
        {
            var song = Parameter;
            if (song != null)
            {
                if (song.Type == SubsonicModelTypeEnum.Song)
                {
                    Handle(new PlayFile {Id = song.Id});
                    State = PlaybackViewModelStateEnum.Audio;
                }
                else
                {
                    Source = SubsonicService.GetUriForFileWithId(song.Id);
                    _shellViewModel.Source = null;
                    State = PlaybackViewModelStateEnum.Video;
                }
            }
        }

        public void StartPlayback(object sender, object e)
        {
            _shellViewModel.Source = ((PlaylistItemViewModel)(((ItemClickEventArgs) e).ClickedItem)).Uri;
        }

        public void Handle(PlaylistMessage message)
        {
            foreach (var item in message.Queue)
            {
                var pi = new PlaylistItemViewModel();

                if (item.Type == SubsonicModelTypeEnum.Song || item.Type == SubsonicModelTypeEnum.Video)
                {
                    var model = item as MusicDirectoryChild;
                    pi.Artist = model.Artist;
                    pi.CoverArt = SubsonicService.GetCoverArtForId(model.CoverArt);
                    pi.Duration = model.Duration;
                    pi.Title = model.Title;
                    pi.Uri = item.Type == SubsonicModelTypeEnum.Song
                                 ? SubsonicService.GetUriForFileWithId(item.Id)
                                 : SubsonicService.GetUriForVideoWithId(item.Id);

                    Playlist.Add(item);
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
            _shellViewModel.Source = SubsonicService.GetUriForFileWithId(message.Id);
        }

        public void Handle(PlayNextMessage message)
        {
            _currentTrackNo++;
            if (_currentTrackNo < Playlist.Count)
            {
                Handle(new PlayFile { Id = Playlist[_currentTrackNo].Id });
            }
            else
            {
                StopAndReset();
            }
        }

        public void Handle(PlayPreviousMessage message)
        {
            _currentTrackNo--;
            if (_currentTrackNo > -1)
                Handle(new PlayFile { Id = Playlist[_currentTrackNo].Id });
            else
            {
                StopAndReset();
            }
        }

        public void Handle(PlayPauseMessage message)
        {
            if (Source != null || _shellViewModel.Source != null)
                _shellViewModel.PlayPause();
            else
                Handle(new PlayNextMessage());
        }

        public void Handle(StopMessage message)
        {
            _shellViewModel.Stop();
        }

        private void StopAndReset()
        {
            _currentTrackNo = 0;
            _shellViewModel.Source = null;
        }
    }
}