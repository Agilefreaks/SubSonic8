namespace Subsonic8.VideoPlayback
{
    using Subsonic8.Framework.Services;

    public class EmbededVideoPlaybackViewModel : VideoPlaybackViewModel, IEmbededVideoPlaybackViewModel
    {
        #region Constructors and Destructors

        public EmbededVideoPlaybackViewModel(IToastNotificationService notificationService)
            : base(notificationService)
        {
        }

        #endregion
    }
}