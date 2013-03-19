using System;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.VideoPlayback
{
    public interface IVideoPlaybackViewModel : IViewModel
    {
        Uri Source { get; set; }

        TimeSpan StartTime { get; set; }

        TimeSpan EndTime { get; set; }
    }
}