using System;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.VideoPlayback
{
    public interface IFullScreenVideoPlaybackViewModel : IViewModel, IPlaybackControlsViewModel, IToastNotificationCapable,
        IHandle<StartVideoPlaybackMessage>, IHandle<StopVideoPlaybackMessage>
    {
        Uri Source { get; set; }

        TimeSpan StartTime { get; set; }

        TimeSpan EndTime { get; set; }
    }
}