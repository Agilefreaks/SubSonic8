namespace Subsonic8.VideoPlayback
{
    using System;
    using Subsonic8.Framework.Interfaces;

    public interface IVideoPlayerView : IPlayerControls
    {
        #region Public Properties

        Action<TimeSpan> SetEndTimeAction { get; }

        Action<TimeSpan> SetStartTimeAction { get; }

        #endregion
    }
}