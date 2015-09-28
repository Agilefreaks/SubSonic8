namespace Client.Common.Helpers
{
    using System;
    using Windows.Media;
    using Windows.Storage.Streams;
    using Caliburn.Micro;
    using Client.Common.Models;
    using EventAggregatorMessages;

    public class MediaControlHandler : IMediaControlHandler 
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;
        private SystemMediaTransportControls _mediaTransportControls;

        #endregion

        #region Constructors and Destructors

        public MediaControlHandler(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region Public Methods and Operators

        public void Register()
        {
            _eventAggregator.Subscribe(this);
            _mediaTransportControls = SystemMediaTransportControls.GetForCurrentView();
            _mediaTransportControls.DisplayUpdater.AppMediaId = "Subsonic8";
            _mediaTransportControls.IsPlayEnabled = true;
            _mediaTransportControls.IsPauseEnabled = true;
            _mediaTransportControls.IsStopEnabled = true;
            _mediaTransportControls.IsNextEnabled = true;
            _mediaTransportControls.IsPreviousEnabled = true;
            _mediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
            _mediaTransportControls.ButtonPressed += MediaTransportControlsOnButtonPressed;
        }

        private void MediaTransportControlsOnButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                case SystemMediaTransportControlsButton.Pause:
                    _eventAggregator.Publish(new PlayPauseMessage());
                    break;
                case SystemMediaTransportControlsButton.Stop:
                    _eventAggregator.Publish(new StopMessage());
                    break;
                case SystemMediaTransportControlsButton.Next:
                    _eventAggregator.Publish(new JumpToNextMessage());
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    _eventAggregator.Publish(new JumpToPreviousMessage());
                    break;
            }
        }

        public void Handle(StartPlaybackMessage message)
        {
            _mediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            SetItemMetaData(message.Item);
        }

        public void Handle(StopPlaybackMessage message)
        {
            _mediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
        }

        public void Handle(PausePlaybackMessage message)
        {
            _mediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Paused;
        }

        public void Handle(ResumePlaybackMessage message)
        {
            _mediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            SetItemMetaData(message.Item);
        }

        #endregion

        #region Methods

        private void SetItemMetaData(PlaylistItem item)
        {
            if (item.Type == PlaylistItemTypeEnum.Audio)
            {
                _mediaTransportControls.DisplayUpdater.Type = MediaPlaybackType.Music;
                _mediaTransportControls.DisplayUpdater.MusicProperties.Artist = item.Artist;
                _mediaTransportControls.DisplayUpdater.MusicProperties.Title = item.Title;
                _mediaTransportControls.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(item.CoverArtUrl));
            }
            else
            {
                _mediaTransportControls.DisplayUpdater.Type = MediaPlaybackType.Video;
                _mediaTransportControls.DisplayUpdater.VideoProperties.Title = item.Title;
            }
            _mediaTransportControls.DisplayUpdater.Update();
        }

        #endregion
    }
}