using System;

namespace Subsonic8.Framework.Interfaces
{
    public interface IPlayerControls
    {
        Action Stop { get; }

        Action Play { get; }

        Action Pause { get; }
    }
}