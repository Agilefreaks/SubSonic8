using System;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Playback;

namespace Subsonic8.VideoPlayback
{
    public class FullScreenVideoPlaybackViewModel : PlaybackControlsViewModelBase, IFullScreenVideoPlaybackViewModel
    {
        private readonly IToastNotificationService _notificationService;
        private Uri _source;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public IToastNotificationService ToastNotificationService { get { return _notificationService; } }

        protected Client.Common.Models.PlaylistItem Item { get; set; }

        public FullScreenVideoPlaybackViewModel(IEventAggregator eventAggregator, IToastNotificationService notificationService)
            : base(eventAggregator)
        {
            _notificationService = notificationService;
        }

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

        public void GoToPlaybackViewModelItem(MediaPlayer mediaPlayer)
        {
            mediaPlayer.Pause();
            var startTime = mediaPlayer.EndTime - mediaPlayer.TimeRemaining;
            EventAggregator.Publish(new StartVideoPlaybackMessage(Item)
            {
                FullScreen = false,
                EndTime = mediaPlayer.TimeRemaining.TotalSeconds,
                StartTime = startTime.TotalSeconds
            });
            NavigationService.NavigateToViewModel<PlaybackViewModel>();
        }

        public void Handle(StartVideoPlaybackMessage message)
        {
            if(!message.FullScreen) return;
            Item = message.Item;
            Source = SubsonicService.GetUriForVideoStartingAt(message.Item.Uri, message.StartTime);
            StartTime = TimeSpan.FromSeconds(message.StartTime).Negate();
            EndTime = TimeSpan.FromSeconds(message.EndTime);
            this.ShowToast(message.Item);
        }

        public void Handle(StopVideoPlaybackMessage message)
        {
            Source = null;
        }
    }
}