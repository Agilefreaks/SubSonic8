namespace Subsonic8.VideoPlayback
{
    using Microsoft.PlayerFramework;

    public sealed partial class EmbededVideoPlaybackView
    {
        #region Constructors and Destructors

        public EmbededVideoPlaybackView()
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