using Microsoft.PlayerFramework;
using Windows.UI.Xaml;

namespace Subsonic8.VideoPlayback
{
    public sealed partial class EmbededVideoPlaybackView
    {
        public EmbededVideoPlaybackView()
        {
            InitializeComponent();
        }

        private void MediaPlayer_OnMediaEnded(object sender, MediaPlayerActionEventArgs e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((EmbededVideoPlaybackViewModel)DataContext).Next();
        }

        private void MediaPlayer_OnIsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            // TODO: Replace with something nicer | It may be bug in Windows.Interactivity
            ((EmbededVideoPlaybackViewModel)DataContext).FullScreenChanged(MediaPlayer);
        }
    }
}
