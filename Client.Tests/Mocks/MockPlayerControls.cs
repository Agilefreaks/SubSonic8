using System;
using Subsonic8.Framework.Interfaces;
using Windows.UI.Xaml;

namespace Client.Tests.Mocks
{
    internal class MockPlayerControls : IPlayerControls
    {
        public event RoutedEventHandler PlayNextClicked;

        public event RoutedEventHandler PlayPreviousClicked;

        public Action PlayPause { get; private set; }

        public Action StopAction { get; private set; }

        public Action PlayAction { get; private set; }

        public Action PauseAction { get; private set; }

        public int PlayPauseCallCount { get; set; }

        public int StopCallCount { get; set; }

        public MockPlayerControls()
        {
            PlayPause = PlayPauseImpl;
            StopAction = StopImplementation;
        }

        private void StopImplementation()
        {
            StopCallCount++;
        }

        private void PlayPauseImpl()
        {
            PlayPauseCallCount++;
        }
    }
}