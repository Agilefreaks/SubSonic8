using System;

namespace Subsonic8.VideoPlayback
{
    public class VideoStartInfo
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public Uri Source { get; set; }
    }
}
