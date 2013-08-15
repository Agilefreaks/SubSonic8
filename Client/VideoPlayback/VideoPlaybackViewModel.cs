namespace Subsonic8.VideoPlayback
{
    using System;
    using System.Collections.Generic;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Services.DataStructures.PlayerManagementService;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Services;
    using Subsonic8.Framework.ViewModel;
    using Windows.UI.Xaml;
    using PlaylistItem = Client.Common.Models.PlaylistItem;

    public class VideoPlaybackViewModel : PlaybackControlsViewModelBase, IVidePlaybackViewModel
    {
        #region Fields

        private TimeSpan _endTime;

        private List<Action> _pendingPlayerActions;

        private IVideoPlayerView _playerControls;

        private Uri _source;

        private TimeSpan _startTime;

        #endregion

        #region Cosntructors

        public VideoPlaybackViewModel()
        {
            _pendingPlayerActions = new List<Action>();
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

        [Inject]
        public IToastNotificationService ToastNotificationService { get; set; }

        #endregion

        #region Public Methods and Operators

        public void OnFullScreenChanged()
        {
            if (FullScreenChanged != null)
            {
                FullScreenChanged(
                    this,
                    new PlaybackStateEventArgs
                        {
                            StartTime = _playerControls.GetStartTime(),
                            EndTime = _playerControls.GetEndTime(),
                            TimeRemaining = _playerControls.GetTimeRemaining()
                        });
            }
        }

        public void SongFailed(ExceptionRoutedEventArgs eventArgs)
        {
            EventAggregator.Publish(new PlayFailedMessage(eventArgs.ErrorMessage, eventArgs.OriginalSource));
        }

        #endregion

        #region Explicit Interface Methods

        void IPlayer.Pause()
        {
            if (_playerControls != null)
            {
                _playerControls.Pause();
            }
        }

        void IPlayer.Play(PlaylistItem item, object options)
        {
            var startInfo = GetStartInfo(item, options as PlaybackStateEventArgs);
            Source = startInfo.Source;
            _pendingPlayerActions = new List<Action>
                                        {
                                            () => _playerControls.SetStartTime(startInfo.StartTime.Negate()), 
                                            () => _playerControls.SetEndTime(startInfo.EndTime), 
                                            () => _playerControls.Play()
                                        };
            ExecutePendingPlayerActions();
        }

        void IPlayer.Resume()
        {
            if (_playerControls != null)
            {
                _playerControls.Play();
            }
        }

        void IPlayer.Stop()
        {
            if (_playerControls != null)
            {
                _playerControls.Pause();
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