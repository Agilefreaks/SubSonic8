namespace Subsonic8.AudioPlayback
{
    using System;

    public interface IPlaybackReporter
    {
        TimeSpan GetCurrentPosition();

        TimeSpan GetDuration();

        #region Public Events

        event EventHandler<EventArgs> PlaybackPaused;

        event EventHandler<EventArgs> PlaybackStarted;

        event EventHandler<EventArgs> PlaybackStoped;

        #endregion
    }
}