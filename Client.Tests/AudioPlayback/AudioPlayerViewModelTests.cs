namespace Client.Tests.AudioPlayback
{
    using System;

    using Windows.UI.Xaml;

    using Caliburn.Micro;

    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Tests.Mocks;

    using FluentAssertions;

    using global::Common.Mocks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Subsonic8.AudioPlayback;

    [TestClass]
    public class AudioPlayerViewModelTests
    {
        #region Fields

        private MockEventAggregator _mockEventAggregator;

        private MockExtendedPlayerControls _mockPlayerControls;

        private AudioPlayerViewModel _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Setup()
        {
            IoC.GetInstance = (type, s) => null;
            _mockPlayerControls = new MockExtendedPlayerControls();
            _mockEventAggregator = new MockEventAggregator();
            _subject = new AudioPlayerViewModel
                           {
                               EventAggregator = _mockEventAggregator,
                               PlayerControls = _mockPlayerControls
                           };
        }

        [TestMethod]
        public void Stop_Always_CallsPlayerControlsStop()
        {
            _subject.Stop();

            _mockPlayerControls.StopCallCount.Should().Be(1);
        }
        
        [TestMethod]
        public void Play_Always_CallsPlayerControlsPlay()
        {
            _subject.Play(new PlaylistItem());

            _mockPlayerControls.PlayCallCount.Should().Be(1);
        }

        [TestMethod]
        public void Play_Always_SetsTheItemUriAsTheSource()
        {
            var uri = new Uri("http://test.com");
            _subject.Play(new PlaylistItem { Uri = uri});

            _subject.Source.Should().Be(uri);
        }

        [TestMethod]
        public void SongEnded_Always_SendsAPlayNextMessage()
        {
            _subject.SongEnded();

            _mockEventAggregator.Messages.Count.Should().Be(1);
            _mockEventAggregator.Messages[0].Should().BeOfType<PlayNextMessage>();
        }

        #endregion
    }
}