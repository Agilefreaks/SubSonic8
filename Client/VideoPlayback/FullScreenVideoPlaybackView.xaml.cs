using Microsoft.PlayerFramework;
using Windows.UI.Xaml;

namespace Subsonic8.VideoPlayback
{
    public sealed partial class FullScreenVideoPlaybackView
    {
        public FullScreenVideoPlaybackView()
        {
            InitializeComponent();
        }

        private void MediaPlayer_OnMediaEnded(object sender, MediaPlayerActionEventArgs e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((FullScreenVideoPlaybackViewModel)DataContext).Next();
        }

        private void MediaPlayer_OnIsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((FullScreenVideoPlaybackViewModel)DataContext).FullScreenChanged(MediaPlayer);
        }
    }
}
