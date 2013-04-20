using System;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Interfaces;
using Windows.UI.Xaml;

namespace Subsonic8.VideoPlayback
{
    public sealed partial class EmbededVideoPlaybackView : IPlayerControls
    {
        public Action StopAction { get; private set; }

        public Action PlayAction { get; private set; }

        public Action PauseAction { get; private set; }

        public EmbededVideoPlaybackView()
        {
            InitializeComponent();
            StopAction = Stop;
            PlayAction = Play;
            PauseAction = Pause;
        }

        private void Pause()
        {
            MediaPlayer.Pause();
        }

        private void Play()
        {
            MediaPlayer.Play();
        }

        private void Stop()
        {
            MediaPlayer.Stop();
        }

        private void MediaPlayer_OnMediaEnded(object sender, MediaPlayerActionEventArgs e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((EmbededVideoPlaybackViewModel)DataContext).Next();
        }

        private void MediaPlayer_OnIsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((EmbededVideoPlaybackViewModel)DataContext).OnFullScreenChanged(MediaPlayer);
        }
    }
}
