namespace Subsonic8.VideoPlayback
{
    using System;
    using Microsoft.PlayerFramework;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public abstract class VideoPlaybackView : Page, IVideoPlayerView
    {
        #region Constructors and Destructors

        protected VideoPlaybackView()
        {
            StopAction = Stop;
            PlayAction = Play;
            PauseAction = Pause;
            SetStartTimeAction = SetStartTime;
            SetEndTimeAction = SetEndTime;
        }

        #endregion

        #region Public Properties

        public Action PauseAction { get; private set; }

        public Action PlayAction { get; private set; }

        public Action<TimeSpan> SetEndTimeAction { get; private set; }

        public Action<TimeSpan> SetStartTimeAction { get; private set; }

        public Action StopAction { get; private set; }

        #endregion

        #region Properties

        protected abstract MediaPlayer GetMediaPlayer { get; }

        #endregion

        #region Methods

        protected void MediaPlayer_OnIsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> eventArgs)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((IVidePlaybackViewModel)DataContext).OnFullScreenChanged(GetMediaPlayer);
        }

        protected void MediaPlayer_OnMediaEnded(object sender, MediaPlayerActionEventArgs eventArgs)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((IVidePlaybackViewModel)DataContext).Next();
        }

        protected void MediaPlayer_OnMediaFailed(object sender, ExceptionRoutedEventArgs eventArgs)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((IVidePlaybackViewModel)DataContext).SongFailed(eventArgs);
        }

        private void Pause()
        {
            GetMediaPlayer.Pause();
        }

        private void Play()
        {
            var mediaPlayer = GetMediaPlayer;
            mediaPlayer.AutoPlay = true;
            mediaPlayer.Play();
        }

        private void SetEndTime(TimeSpan endTime)
        {
            GetMediaPlayer.EndTime = endTime;
        }

        private void SetStartTime(TimeSpan startTime)
        {
            GetMediaPlayer.StartTime = startTime;
        }

        private void Stop()
        {
            var mediaPlayer = GetMediaPlayer;
            mediaPlayer.AutoPlay = false;
            mediaPlayer.Stop();
        }

        #endregion
    }
}