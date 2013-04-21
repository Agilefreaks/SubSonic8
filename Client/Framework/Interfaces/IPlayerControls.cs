using System;

namespace Subsonic8.Framework.Interfaces
{
    public interface IPlayerControls
    {
        Action StopAction { get; }

        Action PlayAction { get; }

        Action PauseAction { get; }
    }
}