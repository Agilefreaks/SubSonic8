namespace Subsonic8.VideoPlayback
{
    using System;
    using Microsoft.PlayerFramework;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public abstract class VideoPlaybackView : Page, IVideoPlayerView
    {
        #region Properties

        protected abstract MediaPlayer GetMediaPlayer { get; }

        #endregion

        #region Methods

        public void Pause()
        {
            GetMediaPlayer.Pause();
        }

        public void Play()
        {
            var mediaPlayer = GetMediaPlayer;
            mediaPlayer.AutoPlay = true;
            mediaPlayer.Play();
        }

        public void SetEndTime(TimeSpan endTime)
        {
            GetMediaPlayer.EndTime = endTime;
        }

        public void SetStartTime(TimeSpan startTime)
        {
            GetMediaPlayer.StartTime = startTime;
        }

        public void Stop()
        {
            var mediaPlayer = GetMediaPlayer;
            mediaPlayer.AutoPlay = false;
            mediaPlayer.Stop();
        }

        public TimeSpan GetEndTime()
        {
            return GetMediaPlayer.EndTime;
        }

        public TimeSpan GetStartTime()
        {
            return GetMediaPlayer.StartTime;
        }

        public TimeSpan GetTimeRemaining()
        {
            return GetMediaPlayer.TimeRemaining;
        }

        protected void MediaPlayer_OnIsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> eventArgs)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((IVidePlaybackViewModel)DataContext).OnFullScreenChanged();
        }

        protected void MediaPlayer_OnMediaEnded(object sender, MediaPlayerActionEventArgs eventArgs)
        {
            GetMediaPlayer.Source = null;
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((IVidePlaybackViewModel)DataContext).Next();
        }

        protected void MediaPlayer_OnMediaFailed(object sender, ExceptionRoutedEventArgs eventArgs)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((IVidePlaybackViewModel)DataContext).SongFailed(eventArgs);
        }

        #endregion
    }
}