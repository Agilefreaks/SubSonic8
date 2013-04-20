using Client.Common.EventAggregatorMessages;
using Client.Common.Helpers;
using Client.Common.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Helpers
{
    [TestClass]
    public class MediaControlHandlerTests
    {
        MediaControlHandler _subject;
        private MockEventAggregator _eventAggregator;

        [TestInitialize]
        public void Setup()
        {
            _eventAggregator = new MockEventAggregator();
            _subject = new MediaControlHandler(_eventAggregator);
        }

        [TestMethod]
        public void PlayNextShouldCallPublishOnEventAggregator()
        {
            _subject.PlayNextTrackPressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<PlayNextMessage>();
        }

        [TestMethod]
        public void PlayPreviousShouldCallPublishOnEventAggregator()
        {
            _subject.PlayPreviousTrackPressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<PlayPreviousMessage>();
        }

        [TestMethod]
        public void PlayPausePressed_Always_ShouldPublishANewPlayPuaseMessage()
        {
            _subject.PlayPressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<PlayPauseMessage>();
        }

        [TestMethod]
        public void PlayPressed_Always_ShouldPublishANewPlayPuaseMessage()
        {
            _subject.PlayPressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<PlayMessage>();
        }

        [TestMethod]
        public void PausePressed_Always_ShouldPublishANewPlayPuaseMessage()
        {
            _subject.PausePressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<PauseMessage>();
        }

        [TestMethod]
        public void StopPressed_Always_ShouldPublishANewStopPlaybackMessage()
        {
            _subject.StopPressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<StopMessage>();
        }
    }
}