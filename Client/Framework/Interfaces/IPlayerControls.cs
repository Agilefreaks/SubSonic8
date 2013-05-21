namespace Subsonic8.Framework.Interfaces
{
    using System;

    public interface IPlayerControls
    {
        #region Public Properties

        Action PauseAction { get; }

        Action PlayAction { get; }

        Action StopAction { get; }

        #endregion
    }
}