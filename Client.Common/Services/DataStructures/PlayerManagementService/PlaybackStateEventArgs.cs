namespace Client.Common.Services.DataStructures.PlayerManagementService
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