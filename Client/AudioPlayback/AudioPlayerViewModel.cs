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

        private IPlayerControls _playerControls;

        private Uri _source;

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

        public IPlayerControls PlayerControls
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

        public void Pause()
        {
            _playerControls.Pause();
        }

        public void Play(PlaylistItem item, object options = null)
        {
            Source = item.Uri;
            _playerControls.Play();
        }

        public void Resume()
        {
            _playerControls.Play();
        }

        public void SongEnded()
        {
            _eventAggregator.Publish(new PlayNextMessage());
        }

        public void SongFailed(ExceptionRoutedEventArgs eventArgs)
        {
            _eventAggregator.Publish(new PlayFailedMessage(eventArgs.ErrorMessage, eventArgs.OriginalSource));
        }

        public void Stop()
        {
            _playerControls.Stop();
        }

        #endregion

        #region Methods

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            PlayerControls = (IPlayerControls)view;
        }

        #endregion
    }
}