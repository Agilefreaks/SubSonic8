using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Services;

namespace Subsonic8.VideoPlayback
{
    public class EmbededVideoPlaybackViewModel : VideoPlaybackViewModel, IEmbededVideoPlaybackViewModel
    {
        public EmbededVideoPlaybackViewModel(IEventAggregator eventAggregator, IToastNotificationService notificationService)
            : base(eventAggregator, notificationService)
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
