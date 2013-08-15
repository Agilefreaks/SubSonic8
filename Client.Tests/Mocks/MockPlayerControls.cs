namespace Client.Tests.Mocks
{
    using System;
    using Subsonic8.Framework.Interfaces;

    internal class MockPlayerControls : IPlayerControls
    {
        #region Public Properties

        public int PlayPauseCallCount { get; set; }

        public int StopCallCount { get; set; }

        #endregion

        #region Public Methods and Operators

        public void SetSource(Uri source)
        {
        }

        #endregion

        #region Methods

        public void PlayPause()
        {
            PlayPauseCallCount++;
        }

        public void Stop()
        {
            StopCallCount++;
        }

        public void Pause()
        {
        }

        public void Play()
        {
        }

        #endregion
    }
}