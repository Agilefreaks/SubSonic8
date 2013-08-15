namespace Subsonic8.VideoPlayback
{
    using Microsoft.PlayerFramework;

    public sealed partial class SnappedVideoPlaybackView
    {
        #region Constructors and Destructors

        public SnappedVideoPlaybackView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        protected override MediaPlayer GetMediaPlayer
        {
            get
            {
                return MediaPlayer;
            }
        }

        #endregion
    }
}