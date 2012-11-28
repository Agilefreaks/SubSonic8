using System;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.ViewModels;
using Subsonic8.Messages;

namespace Subsonic8.Playback
{
    public class PlaybackViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly IEventAggregator _eventAggregator;
        private ISubsonicModel _parameter;
        private PlaybackViewModelStateEnum _state;
        private Uri _source;

        #endregion

        #region Public Properties

        public ISubsonicModel Parameter
        {
            get { return _parameter; }

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
            get { return _source; }

            set
            {
                if (_source == value) return;
                _source = value;
                NotifyOfPropertyChange(() => Source);
            }
        }

        public PlaybackViewModelStateEnum State
        {
            get { return _state; }

            set
            {
                if (_state == value) return;
                _state = value;
                NotifyOfPropertyChange(() => State);
            }
        }

        #endregion

        public PlaybackViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        private void StartPlayback()
        {
            if (Parameter.Type == SubsonicModelTypeEnum.Video)
            {
                Source = SubsonicService.GetUriForVideoWithId(Parameter.Id);
                State = PlaybackViewModelStateEnum.Video;
            }
            else
            {
                _eventAggregator.Publish(new PlayFile { Id = Parameter.Id });
                State = PlaybackViewModelStateEnum.Audio;
            }
        }
    }
}