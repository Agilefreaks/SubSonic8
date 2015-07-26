namespace Subsonic8.AudioPlayback
{
    using System;

    using Windows.UI.Xaml;

    using Caliburn.Micro;

    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;

    using MugenInjection.Attributes;

    using Subsonic8.Framework.Interfaces;

    public class AudioPlayerViewModel : Screen, IAudioPlayerViewModel
    {
        #region Fields

        private IEventAggregator _eventAggregator;

        private IExtendedPlayerControls _playerControls;

        private Uri _source;

        private PlaylistItem _currentItem;

        #endregion

        #region Public Events

        public event EventHandler<EventArgs> PlaybackPaused;

        public event EventHandler<EventArgs> PlaybackStarted;

        public event EventHandler<EventArgs> PlaybackStoped;

        #endregion

        #region Public Properties

        [Inject]
        public IEventAggregator EventAggregator
        {
            get
            {
                return _eventAggregator;
            }

            set
            {
                _eventAggregator = value;
                _eventAggregator.Subscribe(this);
            }
        }

        public IExtendedPlayerControls PlayerControls
        {
            get
            {
                return _playerControls;
            }

            set
            {
                _playerControls = value;
                NotifyOfPropertyChange();
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
                _source = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        public TimeSpan GetCurrentPosition()
        {
            return _playerControls.GetCurrentPosition();
        }

        public TimeSpan GetDuration()
        {
            return TimeSpan.FromSeconds(_currentItem.Duration);
        }

        public void Pause()
        {
            _playerControls.Pause();
            if (PlaybackPaused != null)
            {
                PlaybackPaused(this, new EventArgs());
            }
        }

        public void Play(PlaylistItem item, object options = null)
        {
            _currentItem = item;
            Source = item.Uri;
            _playerControls.Play();
            if (PlaybackStarted != null)
            {
                PlaybackStarted(this, new EventArgs());
            }
        }

        public void Resume()
        {
            _playerControls.Play();
            if (PlaybackStarted != null)
            {
                PlaybackStarted(this, new EventArgs());
            }
        }

        public void SongEnded()
        {
            if (PlaybackStoped != null)
            {
                PlaybackStoped(this, new EventArgs());
            }
            EventAggregator.Publish(new PlayNextMessage());
        }

        public void SongFailed(ExceptionRoutedEventArgs eventArgs)
        {
            if (PlaybackStoped != null)
            {
                PlaybackStoped(this, new EventArgs());
            }
            EventAggregator.Publish(new PlayFailedMessage(eventArgs.ErrorMessage, eventArgs.OriginalSource));
        }

        public void Stop()
        {
            _playerControls.Stop();
            if (PlaybackStoped != null)
            {
                PlaybackStoped(this, new EventArgs());
            }
        }

        #endregion

        #region Methods

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            PlayerControls = (IExtendedPlayerControls)view;
        }

        #endregion
    }
}