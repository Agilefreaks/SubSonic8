﻿namespace Subsonic8.VideoPlayback
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Services.DataStructures.PlayerManagementService;
    using Microsoft.PlayerFramework;
    using Subsonic8.Framework.Services;
    using Subsonic8.Framework.ViewModel;
    using PlaylistItem = Client.Common.Models.PlaylistItem;

    public class VideoPlaybackViewModel : PlaybackControlsViewModelBase, IVidePlaybackViewModel
    {
        #region Fields

        private readonly IToastNotificationService _notificationService;

        private TimeSpan _endTime;

        private List<Action> _pendingPlayerActions;

        private IVideoPlayerView _playerControls;

        private Uri _source;

        private TimeSpan _startTime;

        #endregion

        #region Constructors and Destructors

        public VideoPlaybackViewModel(IToastNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        #endregion

        #region Public Events

        public event EventHandler<PlaybackStateEventArgs> FullScreenChanged;

        #endregion

        #region Public Properties

        public TimeSpan EndTime
        {
            get
            {
                return _endTime;
            }

            set
            {
                if (value.Equals(_endTime))
                {
                    return;
                }

                _endTime = value;
                NotifyOfPropertyChange(() => EndTime);
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
                if (_source == value)
                {
                    return;
                }

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
                if (value == _startTime)
                {
                    return;
                }

                _startTime = value;
                NotifyOfPropertyChange(() => StartTime);
            }
        }

        public IToastNotificationService ToastNotificationService
        {
            get
            {
                return _notificationService;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void OnFullScreenChanged(MediaPlayer mediaPlayer)
        {
            if (FullScreenChanged != null)
            {
                FullScreenChanged(
                    this, 
                    new PlaybackStateEventArgs
                        {
                            StartTime = mediaPlayer.StartTime, 
                            EndTime = mediaPlayer.EndTime, 
                            TimeRemaining = mediaPlayer.TimeRemaining
                        });
            }
        }

        #endregion

        #region Explicit Interface Methods

        void IPlayer.Pause()
        {
            if (_playerControls != null)
            {
                _playerControls.PauseAction();
            }
        }

        void IPlayer.Play(PlaylistItem item, object options)
        {
            // var regex = new Regex("(.*)(id=)([0-9]{1,})(.*)");
            // var id = int.Parse(regex.Matches(item.UriAsString)[0].Groups[3].Value);
            // await SubsonicService.GetSong(id).WithErrorHandler(this).Execute();
            var startInfo = GetStartInfo(item, options as PlaybackStateEventArgs);
            Source = startInfo.Source;
            _pendingPlayerActions = new List<Action>
                                        {
                                            () =>
                                            _playerControls.SetStartTimeAction(
                                                startInfo.StartTime.Negate()), 
                                            () => _playerControls.SetEndTimeAction(startInfo.EndTime), 
                                            () => _playerControls.PlayAction()
                                        };
            ExecutePendingPlayerActions();
        }

        void IPlayer.Resume()
        {
            if (_playerControls != null)
            {
                _playerControls.PlayAction();
            }
        }

        void IPlayer.Stop()
        {
            if (_playerControls != null)
            {
                _playerControls.PauseAction();
            }
        }

        #endregion

        #region Methods

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _playerControls = view as IVideoPlayerView;
            ExecutePendingPlayerActions();
        }

        private void ExecutePendingPlayerActions()
        {
            if (_playerControls == null)
            {
                return;
            }

            foreach (var action in _pendingPlayerActions)
            {
                action();
            }

            _pendingPlayerActions.Clear();
        }

        private VideoStartInfo GetStartInfo(PlaylistItem item, PlaybackStateEventArgs eventArgs)
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
            videoStartInfo.Source = SubsonicService.GetUriForVideoStartingAt(
                item.Uri, videoStartInfo.StartTime.TotalSeconds);

            return videoStartInfo;
        }

        #endregion
    }
}