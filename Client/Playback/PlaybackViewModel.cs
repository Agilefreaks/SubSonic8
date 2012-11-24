using System;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.ViewModels;
using Subsonic8.Messages;

namespace Subsonic8.Playback
{
    public class PlaybackViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private ISubsonicModel _parameter;
        private PlaybackViewModelStateEnum _state;
        private Uri _source;

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

        private void StartPlayback()
        {
            var song = Parameter as Song;
            if (song == null) return;
            if (song.IsVideo == false)
            {
                _eventAggregator.Publish(new PlayFile { Id = song.Id });
                State = PlaybackViewModelStateEnum.Audio;
            }
            else
            {
                Source = SubsonicService.GetUriForFileWithId(song.Id);
                State = PlaybackViewModelStateEnum.Video;
            }
        }

        public PlaybackViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
    }
}