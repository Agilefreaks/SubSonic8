using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.ViewModels;
using Subsonic8.Messages;
using Subsonic8.Shell;

namespace Subsonic8.Playback
{
    public class PlaybackViewModel : ViewModelBase, IPlaybackViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IShellViewModel _shellViewModel;
        private ISubsonicModel _parameter;
        private PlaybackViewModelStateEnum _state;
        private Uri _source;
        private ObservableCollection<ISubsonicModel> _playlist;

        public ISubsonicModel Parameter
        {
            get
            {
                return _parameter;
            }

            set
            {
                if (Equals(value, _parameter)) return;
                _parameter = value;
                NotifyOfPropertyChange();
                StartPlayback();
            }
        }

        public Uri Source
        {
            get
            {
                return _source;
            }

            set
            {
                if(_source == value) return;
                _source = value;
                NotifyOfPropertyChange(() => Source);
            }
        }

        public PlaybackViewModelStateEnum State
        {
            get
            {
                return _state;
            }

            set
            {
                if(_state == value) return;
                _state = value;
                NotifyOfPropertyChange(() => State);
            }
        }

        public ObservableCollection<ISubsonicModel> Playlist
        {
            get { return _playlist; }
            set
            {
                if (Equals(value, _playlist)) return;
                _playlist = value;
                NotifyOfPropertyChange();
            }
        }

        public PlaybackViewModel(IEventAggregator eventAggregator, IShellViewModel shellViewModel)
        {
            _eventAggregator = eventAggregator;
            _shellViewModel = shellViewModel;
            _eventAggregator.Subscribe(this);
            Playlist = new ObservableCollection<ISubsonicModel>();
        }

        public void StartPlayback()
        {
            var song = Parameter;
            if (song == null) return;
            if (song.Type == SubsonicModelTypeEnum.Song)
            {
                Handle(new PlayFile { Id = song.Id });
                State = PlaybackViewModelStateEnum.Audio;
            }
            else
            {
                Source = SubsonicService.GetUriForFileWithId(song.Id);
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
            _shellViewModel.Source = SubsonicService.GetUriForFileWithId(message.Id);
        }
    }
}