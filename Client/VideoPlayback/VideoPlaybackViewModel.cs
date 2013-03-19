using System;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.VideoPlayback
{
    public class VideoPlaybackViewModel : PlaybackControlsViewModelBase, IVidePlaybackViewModel
    {
        private readonly IToastNotificationService _notificationService;
        private Uri _source;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public IToastNotificationService ToastNotificationService { get { return _notificationService; } }

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

        public virtual StartVideoPlaybackMessage Parameter
        {
            set
            {
                if (value == null) return;
                Handle(value);
            }
        }

        protected Client.Common.Models.PlaylistItem Item { get; set; }

        public VideoPlaybackViewModel(IEventAggregator eventAggregator, IToastNotificationService notificationService)
            : base(eventAggregator)
        {
            _notificationService = notificationService;
        }

        public virtual void Handle(StartVideoPlaybackMessage message)
        {
            Item = message.Item;
            StartTime = TimeSpan.FromSeconds(message.StartTime).Negate();
            EndTime = TimeSpan.FromSeconds(message.EndTime);
            Source = SubsonicService.GetUriForVideoStartingAt(message.Item.Uri, message.StartTime);
        }

        public virtual void Handle(StopVideoPlaybackMessage message)
        {
            Source = null;
        }

        public void FullScreenChanged(MediaPlayer mediaPlayer)
        {
            mediaPlayer.Pause();
            var message = GetStartVideoPlaybackMessageWithCurrentPosition(mediaPlayer);
            EventAggregator.Publish(message);
        }

        public virtual StartVideoPlaybackMessage GetStartVideoPlaybackMessageWithCurrentPosition(MediaPlayer mediaPlayer)
        {
            TimeSpan startTime, endTime;
            if (mediaPlayer.TimeRemaining != TimeSpan.Zero)
            {
                startTime = EndTime - mediaPlayer.TimeRemaining;
                endTime = mediaPlayer.TimeRemaining;
            }
            else
            {
                startTime = mediaPlayer.StartTime;
                endTime = mediaPlayer.EndTime;
            }

            var videoPlaybackMessage = new StartVideoPlaybackMessage(Item)
            {
                StartTime = startTime.TotalSeconds,
                EndTime = endTime.TotalSeconds
            };

            return videoPlaybackMessage;
        }
    }
}