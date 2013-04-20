using Subsonic8.Framework.Services;

namespace Subsonic8.VideoPlayback
{
    public class EmbededVideoPlaybackViewModel : VideoPlaybackViewModel, IEmbededVideoPlaybackViewModel
    {
        public EmbededVideoPlaybackViewModel(IToastNotificationService notificationService)
            : base(notificationService)
        {
        }
    }
}
