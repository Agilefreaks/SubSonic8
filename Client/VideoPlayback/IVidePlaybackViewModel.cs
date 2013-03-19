using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.VideoPlayback
{
    public interface IVidePlaybackViewModel : IViewModel, IPlaybackControlsViewModel, IToastNotificationCapable,
                                              IHandle<StartVideoPlaybackMessage>, IHandle<StopVideoPlaybackMessage>
    {
    }
}