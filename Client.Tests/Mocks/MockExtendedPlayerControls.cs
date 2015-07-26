namespace Client.Tests.Mocks
{
    using System;

    using Subsonic8.Framework.Interfaces;

    internal class MockExtendedPlayerControls : MockPlayerControls, IExtendedPlayerControls
    {
        public TimeSpan GetCurrentPosition()
        {
            return TimeSpan.Zero;
        }

        public TimeSpan GetCurrentDuration()
        {
            return TimeSpan.Zero;
        }
    }
}