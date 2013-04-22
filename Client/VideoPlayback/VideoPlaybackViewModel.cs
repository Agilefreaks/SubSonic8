using System;
using System.Collections.Generic;
using Client.Common.Services.DataStructures.PlayerManagementService;
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
        private IVideoPlayerView _playerControls;
        private List<Action> _pendingPlayerActions;

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

        public VideoPlaybackViewModel(IToastNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _playerControls = view as IVideoPlayerView;
            ExecutePendingPlayerActions();
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

        void IPlayer.Play(Client.Common.Models.PlaylistItem item, object options)
        {
            //var regex = new Regex("(.*)(id=)([0-9]{1,})(.*)");
            //var id = int.Parse(regex.Matches(item.UriAsString)[0].Groups[3].Value);
            //await SubsonicService.GetSong(id).WithErrorHandler(this).Execute();
            var startInfo = GetStartInfo(item, options as PlaybackStateEventArgs);
            Source = startInfo.Source;
            _pendingPlayerActions = new List<Action>
                {
                    () => _playerControls.SetStartTimeAction(startInfo.StartTime.Negate()),
                    () => _playerControls.SetEndTimeAction(startInfo.EndTime),
                    () => _playerControls.PlayAction()
                };
            ExecutePendingPlayerActions();
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

        private VideoStartInfo GetStartInfo(Client.Common.Models.PlaylistItem item, PlaybackStateEventArgs eventArgs)
        {
            var videoStartInfo = new VideoStartInfo();
            if (eventArgs == null)
            {
                videoStartInfo.StartTime = TimeSpan.Zero;
                videoStartInfo.EndTime = TimeSpan.FromSeconds(item.Duration);
            }
            else
            {
                if (eventArgs.TimeRemaining != TimeSpan.Zero)
                {
                    videoStartInfo.StartTime = eventArgs.EndTime - eventArgs.TimeRemaining - eventArgs.StartTime;
                    videoStartInfo.EndTime = eventArgs.TimeRemaining;
                }
                else
                {
                    videoStartInfo.StartTime = eventArgs.StartTime;
                    videoStartInfo.EndTime = eventArgs.EndTime;
                }
            }

            videoStartInfo.EndTime = videoStartInfo.EndTime == TimeSpan.Zero
                                         ? TimeSpan.FromSeconds(item.Duration)
                                         : videoStartInfo.EndTime;
            videoStartInfo.Source = SubsonicService.GetUriForVideoStartingAt(item.Uri, videoStartInfo.StartTime.TotalSeconds);

            return videoStartInfo;
        }

        private void ExecutePendingPlayerActions()
        {
            if (_playerControls == null) return;
            foreach (var action in _pendingPlayerActions)
            {
                action();
            }
            _pendingPlayerActions.Clear();
        }
    }
}