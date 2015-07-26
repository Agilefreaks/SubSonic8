namespace Subsonic8.Framework.Interfaces
{
    using System;

    public interface IExtendedPlayerControls : IPlayerControls
    {
        TimeSpan GetCurrentPosition();

        TimeSpan GetCurrentDuration();
    }
}