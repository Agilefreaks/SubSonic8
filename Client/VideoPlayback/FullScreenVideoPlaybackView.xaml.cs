namespace Subsonic8.VideoPlayback
{
    using Microsoft.PlayerFramework;

    public sealed partial class FullScreenVideoPlaybackView
    {
        #region Constructors and Destructors

        public FullScreenVideoPlaybackView()
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