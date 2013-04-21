using System;

namespace Subsonic8.VideoPlayback
{
    public class PlaybackStateEventArgs
    {
        public TimeSpan TimeRemaining { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}