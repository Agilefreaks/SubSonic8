﻿using Microsoft.PlayerFramework;
using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Subsonic8.VideoPlayback
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoPlaybackView
    {
        public VideoPlaybackView()
        {
            InitializeComponent();
        }

        private void MediaPlayer_OnMediaEnded(object sender, MediaPlayerActionEventArgs e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((VideoPlaybackViewModel)DataContext).GoBack();
        }

        private void MediaPlayer_OnIsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((VideoPlaybackViewModel)DataContext).GoBack();
        }
    }
}
