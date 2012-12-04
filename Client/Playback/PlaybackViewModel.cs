using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Services;
using Client.Common.ViewModels;
using Subsonic8.Messages;
using Subsonic8.Shell;

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

        #endregion

        public PlaybackViewModel(IEventAggregator eventAggregator, IShellViewModel shellViewModel, ISubsonicService subsonicService)
        {
            _eventAggregator = eventAggregator;
            _shellViewModel = shellViewModel;
            _eventAggregator.Subscribe(this);
            SubsonicService = subsonicService;
            Playlist = new ObservableCollection<ISubsonicModel>();
        }

        public void StartPlayback()
        {
            var song = Parameter;
            if (song.Type == SubsonicModelTypeEnum.Song)
            {
                Handle(new PlayFile { Id = song.Id });
                State = PlaybackViewModelStateEnum.Audio;
            }
            else
            {
                Source = SubsonicService.GetUriForFileWithId(song.Id);
                _shellViewModel.Source = null;
                State = PlaybackViewModelStateEnum.Video;
            }
        }

        public void Handle(PlaylistMessage message)
        {
            foreach (var item in message.Queue)
            {
                Playlist.Add(item);
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
                Handle(new PlayFile { Id = Playlist[_currentTrackNo].Id });
            else
                _shellViewModel.Source = null;
        }

        public void Handle(PlayPreviousMessage message)
        {
            _currentTrackNo--;
            if (_currentTrackNo > -1)
                Handle(new PlayFile { Id = Playlist[_currentTrackNo].Id });
            else
                _shellViewModel.Source = null;
        }
    }
}