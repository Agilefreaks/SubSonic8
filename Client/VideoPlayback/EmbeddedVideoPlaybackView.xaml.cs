namespace Subsonic8.VideoPlayback
{
    using Microsoft.PlayerFramework;

    public sealed partial class EmbeddedVideoPlaybackView
    {
        #region Constructors and Destructors

        public EmbeddedVideoPlaybackView()
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