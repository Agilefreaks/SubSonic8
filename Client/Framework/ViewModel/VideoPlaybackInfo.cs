using System;

namespace Subsonic8.Framework.ViewModel
{
    public class VideoPlaybackInfo
    {
        public Uri Source { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}