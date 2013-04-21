using System;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.VideoPlayback
{
    public abstract class VideoPlaybackView : Page, IPlayerControls
    {
        public Action StopAction { get; private set; }

        public Action PlayAction { get; private set; }

        public Action PauseAction { get; private set; }

        protected abstract MediaPlayer GetMediaPlayer { get; }

        protected VideoPlaybackView()
        {
            StopAction = Stop;
            PlayAction = Play;
            PauseAction = Pause;
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
            GetMediaPlayer.Play();
        }

        private void Stop()
        {
            GetMediaPlayer.Stop();
        }
    }
}