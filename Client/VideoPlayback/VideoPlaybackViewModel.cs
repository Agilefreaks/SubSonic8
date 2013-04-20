using System;
using Client.Common.Services.DataStructures.PlayerManagementService;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Interfaces;
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
        private IPlayerControls _playerControls;

        public event EventHandler<PlaybackStateEventArgs> FullScreenChanged;

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

        protected Client.Common.Models.PlaylistItem Item { get; set; }

        public VideoPlaybackViewModel(IToastNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _playerControls = view as IPlayerControls;
        }

        public void OnFullScreenChanged(MediaPlayer mediaPlayer)
        {
            if (FullScreenChanged != null)
            {
                FullScreenChanged(this, new PlaybackStateEventArgs
                    {
                        StartTime = mediaPlayer.StartTime,
                        EndTime = mediaPlayer.EndTime,
                        TimeRemaining = mediaPlayer.TimeRemaining
                    });
            }
        }

        void IPlayer.Play(Client.Common.Models.PlaylistItem item)
        {
            Item = item;
            StartTime = TimeSpan.FromSeconds(0).Negate();
            EndTime = TimeSpan.FromSeconds(item.Duration);
            Source = SubsonicService.GetUriForVideoStartingAt(item.Uri, 0);
            if (_playerControls != null) _playerControls.PlayAction();
        }

        void IPlayer.Pause()
        {
            if (_playerControls != null) _playerControls.PauseAction();
        }

        void IPlayer.Resume()
        {
            if (_playerControls != null) _playerControls.PlayAction();
        }

        void IPlayer.Stop()
        {
            if (_playerControls != null) _playerControls.PauseAction();
        }
    }
}