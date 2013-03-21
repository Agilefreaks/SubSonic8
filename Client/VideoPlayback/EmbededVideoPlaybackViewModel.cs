using Client.Common.EventAggregatorMessages;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Services;

namespace Subsonic8.VideoPlayback
{
    public class EmbededVideoPlaybackViewModel : VideoPlaybackViewModel, IEmbededVideoPlaybackViewModel
    {
        public EmbededVideoPlaybackViewModel(IToastNotificationService notificationService)
            : base(notificationService)
        {
        }

        public override StartVideoPlaybackMessage GetStartVideoPlaybackMessageWithCurrentPosition(MediaPlayer mediaPlayer)
        {
            var message = base.GetStartVideoPlaybackMessageWithCurrentPosition(mediaPlayer);
            message.FullScreen = true;

            return message;
        }

        public override void Handle(StartVideoPlaybackMessage message)
        {
            if (!message.FullScreen)
            {
                base.Handle(message);
            }
        }
    }
}
