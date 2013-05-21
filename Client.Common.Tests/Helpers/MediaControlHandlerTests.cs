﻿namespace Client.Common.Tests.Helpers
{
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Helpers;
    using Client.Common.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class MediaControlHandlerTests
    {
        #region Fields

        private MockEventAggregator _eventAggregator;

        private MediaControlHandler _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void PausePressed_Always_ShouldPublishANewPlayPuaseMessage()
        {
            _subject.PausePressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<PauseMessage>();
        }

        [TestMethod]
        public void PlayNextShouldCallPublishOnEventAggregator()
        {
            _subject.PlayNextTrackPressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<PlayNextMessage>();
        }

        [TestMethod]
        public void PlayPausePressed_Always_ShouldPublishANewPlayPuaseMessage()
        {
            _subject.PlayPausePressed(null, null);

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
        public void PlayPreviousShouldCallPublishOnEventAggregator()
        {
            _subject.PlayPreviousTrackPressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<PlayPreviousMessage>();
        }

        [TestInitialize]
        public void Setup()
        {
            _eventAggregator = new MockEventAggregator();
            _subject = new MediaControlHandler(_eventAggregator);
        }

        [TestMethod]
        public void StopPressed_Always_ShouldPublishANewStopPlaybackMessage()
        {
            _subject.StopPressed(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<StopMessage>();
        }

        #endregion
    }
}