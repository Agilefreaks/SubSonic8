using Microsoft.PlayerFramework;

namespace Subsonic8.VideoPlayback
{
    public sealed partial class FullScreenVideoPlaybackView
    {
        protected override MediaPlayer GetMediaPlayer
        {
            get { return MediaPlayer; }
        }

        public FullScreenVideoPlaybackView()
        {
            InitializeComponent();
        }
    }
}
