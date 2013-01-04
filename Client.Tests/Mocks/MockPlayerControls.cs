using System;
using Subsonic8.Shell;
using Windows.UI.Xaml;

namespace Client.Tests.Mocks
{
    internal class MockPlayerControls : IPlayerControls
    {
        public event RoutedEventHandler PlayNextClicked;
        
        public event RoutedEventHandler PlayPreviousClicked;

        public Action PlayPause { get; private set; }

        public int PlayPauseCallCount { get; set; }

        public MockPlayerControls()
        {
            PlayPause = PlayPauseImpl;
        }

        private void PlayPauseImpl()
        {
            PlayPauseCallCount++;
        }
    }
}