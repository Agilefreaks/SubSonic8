using System;
using Microsoft.PlayerFramework;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.VideoPlayback
{
    public abstract class VideoPlaybackView : Page, IVideoPlayerView
    {
        public Action StopAction { get; private set; }

        public Action PlayAction { get; private set; }

        public Action PauseAction { get; private set; }

        public Action<TimeSpan> SetStartTimeAction { get; private set; }

        public Action<TimeSpan> SetEndTimeAction { get; private set; }

        protected abstract MediaPlayer GetMediaPlayer { get; }

        protected VideoPlaybackView()
        {
            StopAction = Stop;
            PlayAction = Play;
            PauseAction = Pause;
            SetStartTimeAction = SetStartTime;
            SetEndTimeAction = SetEndTime;
        }

        protected void MediaPlayer_OnMediaEnded(object sender, MediaPlayerActionEventArgs e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((IVidePlaybackViewModel)DataContext).Next();
        }

        protected void MediaPlayer_OnIsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((IVidePlaybackViewModel)DataContext).OnFullScreenChanged(GetMediaPlayer);
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

        private void Stop()
        {
            var mediaPlayer = GetMediaPlayer;
            mediaPlayer.AutoPlay = false;
            mediaPlayer.Stop();
        }

        private void SetStartTime(TimeSpan startTime)
        {
            GetMediaPlayer.StartTime = startTime;
        }

        private void SetEndTime(TimeSpan endTime)
        {
            GetMediaPlayer.EndTime = endTime;
        }
    }
}