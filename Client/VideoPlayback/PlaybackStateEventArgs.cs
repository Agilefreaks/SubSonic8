namespace Subsonic8.VideoPlayback
{
    using System;

    public class PlaybackStateEventArgs
    {
        #region Public Properties

        public TimeSpan EndTime { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan TimeRemaining { get; set; }

        #endregion
    }
}