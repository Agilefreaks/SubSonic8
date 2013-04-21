using Subsonic8.Framework.Services;

namespace Subsonic8.VideoPlayback
{
    public class FullScreenVideoPlaybackViewModel : VideoPlaybackViewModel, IFullScreenVideoPlaybackViewModel
    {
        public FullScreenVideoPlaybackViewModel(IToastNotificationService notificationService)
            : base(notificationService)
        {
        }
    }
}