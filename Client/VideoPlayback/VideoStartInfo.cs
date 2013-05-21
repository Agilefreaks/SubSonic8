namespace Subsonic8.VideoPlayback
{
    using System;

    public class VideoStartInfo
    {
        #region Public Properties

        public TimeSpan EndTime { get; set; }

        public Uri Source { get; set; }

        public TimeSpan StartTime { get; set; }

        #endregion
    }
}