namespace Subsonic8.VideoPlayback
{
    using Subsonic8.Framework.Services;

    public class FullScreenVideoPlaybackViewModel : VideoPlaybackViewModel, IFullScreenVideoPlaybackViewModel
    {
        #region Constructors and Destructors

        public FullScreenVideoPlaybackViewModel(IToastNotificationService notificationService)
            : base(notificationService)
        {
        }

        #endregion
    }
}