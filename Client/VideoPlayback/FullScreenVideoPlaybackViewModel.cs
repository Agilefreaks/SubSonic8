using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Microsoft.PlayerFramework;
using Subsonic8.Framework.Services;

namespace Subsonic8.VideoPlayback
{
    public class FullScreenVideoPlaybackViewModel : VideoPlaybackViewModel, IFullScreenVideoPlaybackViewModel
    {
        public override StartVideoPlaybackMessage Parameter
        {
            set
            {
                if (value == null) return;
                base.Handle(value);
            }
        }

        public FullScreenVideoPlaybackViewModel(IEventAggregator eventAggregator, IToastNotificationService notificationService)
            : base(eventAggregator, notificationService)
        {
        }

        public override StartVideoPlaybackMessage GetStartVideoPlaybackMessageWithCurrentPosition(MediaPlayer mediaPlayer)
        {
            var message = base.GetStartVideoPlaybackMessageWithCurrentPosition(mediaPlayer);
            message.FullScreen = false;

            return message;
        }

        public override void Handle(StartVideoPlaybackMessage message)
        {
            if (message.FullScreen)
            {
                if (IsActive)
                {
                    base.Handle(message);
                }
                else
                {
                    NavigationService.NavigateToViewModel<FullScreenVideoPlaybackViewModel>(message);
                }
            }
        }
    }
}