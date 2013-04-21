using Microsoft.PlayerFramework;

namespace Subsonic8.VideoPlayback
{
    public sealed partial class EmbededVideoPlaybackView
    {
        protected override MediaPlayer GetMediaPlayer
        {
            get { return MediaPlayer; }
        }

        public EmbededVideoPlaybackView()
        {
            InitializeComponent();
        }
    }
}
