using System;
using Subsonic8.Framework.Interfaces;

namespace Subsonic8.VideoPlayback
{
    public interface IVideoPlayerView : IPlayerControls
    {
        Action<TimeSpan> SetStartTimeAction { get; }

        Action<TimeSpan> SetEndTimeAction { get; }
    }
}