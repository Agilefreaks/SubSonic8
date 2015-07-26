namespace Client.Tests.Mocks
{
    using System;
    using Subsonic8.Framework.Interfaces;

    internal class MockPlayerControls : IPlayerControls
    {
        #region Public Properties

        public int PlayPauseCallCount { get; set; }

        public int StopCallCount { get; set; }

        public int PlayCallCount { get; set; }

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
            PlayCallCount++;
        }

        #endregion
    }
}