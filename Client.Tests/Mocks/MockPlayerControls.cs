namespace Client.Tests.Mocks
{
    using System;
    using Subsonic8.Framework.Interfaces;
    using Windows.UI.Xaml;

    internal class MockPlayerControls : IPlayerControls
    {
        #region Constructors and Destructors

        public MockPlayerControls()
        {
            PlayPause = PlayPauseImpl;
            StopAction = StopImplementation;
        }

        #endregion

        #region Public Events

        public event RoutedEventHandler PlayNextClicked;

        public event RoutedEventHandler PlayPreviousClicked;

        #endregion

        #region Public Properties

        public Action PauseAction { get; private set; }

        public Action PlayAction { get; private set; }

        public Action PlayPause { get; private set; }

        public int PlayPauseCallCount { get; set; }

        public Action StopAction { get; private set; }

        public int StopCallCount { get; set; }

        #endregion

        #region Public Methods and Operators

        public void SetSource(Uri source)
        {
        }

        #endregion

        #region Methods

        private void PlayPauseImpl()
        {
            PlayPauseCallCount++;
        }

        private void StopImplementation()
        {
            StopCallCount++;
        }

        #endregion
    }
}