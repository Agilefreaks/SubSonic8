using System;

namespace Subsonic8.Framework.Interfaces
{
    public interface IPlayerControls
    {
        Action PlayPause { get; }

        Action Stop { get; }

        Action Play { get; }

        Action Pause { get; }
    }
}