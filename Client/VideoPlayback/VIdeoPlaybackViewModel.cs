using System;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.VideoPlayback
{
    public class VideoPlaybackViewModel : ViewModelBase, IVideoPlaybackViewModel
    {
        private Uri _source;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public Uri Source
        {
            get
            {
                return _source;
            }

            set
            {
                if (_source == value) return;
                _source = value;
                NotifyOfPropertyChange();
            }
        }

        public TimeSpan StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                if (value == _startTime) return;
                _startTime = value;
                NotifyOfPropertyChange(() => StartTime);
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                if (value.Equals(_endTime)) return;
                _endTime = value;
                NotifyOfPropertyChange(() => EndTime);
            }
        }

        public object Parameter
        {
            set
            {
                var videoPlaybackInfo = value as VideoPlaybackInfo;
                if (videoPlaybackInfo == null) return;
                StartTime = videoPlaybackInfo.StartTime;
                EndTime = videoPlaybackInfo.EndTime;
                Source = videoPlaybackInfo.Source;
            }
        }
    }
}