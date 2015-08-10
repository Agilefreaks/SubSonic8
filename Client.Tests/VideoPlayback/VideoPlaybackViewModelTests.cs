namespace Client.Tests.VideoPlayback
{
    using System;

    using Client.Common.Models;
    using Client.Common.Services.DataStructures.PlayerManagementService;
    using Client.Tests.Framework.ViewModel;
    using Client.Tests.Mocks;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Subsonic8.VideoPlayback;

    [TestClass]
    public class VideoPlaybackViewModelTests : ViewModelBaseTests<VideoPlaybackViewModel>
    {
        #region Fields

        private readonly Uri _defaultUri = new Uri("http://test.com");

        private MockToastNotificationService _mockToastNotificationService;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void Play_VideoPlaybackIsInitialized_WillNotExecuteAGetRandomSongsResult()
        {
            MockSubsonicService.IsVideoPlaybackInitialized = true;
            var callCount = 0;
            MockSubsonicService.GetRandomSongs = songCount =>
                {
                    callCount++;
                    return new MockGetRandomSongsResult(songCount);
                };

            ((IPlayer)Subject).Play(new PlaylistItem { Uri = _defaultUri });

            callCount.Should().Be(0);
        }

        [TestMethod]
        public void
            Play_VideoPlaybackIsNotInitialized_AndGetRandomSongsResultWasSuccessfull_WillSetIsVideoPlaybackInitializedToTrue
            ()
        {
            MockSubsonicService.IsVideoPlaybackInitialized = false;
            MockSubsonicService.GetRandomSongs = songCount => new MockGetRandomSongsResult(songCount);

            ((IPlayer)Subject).Play(new PlaylistItem { Uri = _defaultUri });

            MockSubsonicService.IsVideoPlaybackInitialized.Should().BeTrue();
        }

        [TestMethod]
        public void Play_VideoPlaybackIsNotInitialized_WillExecuteAGetRandomSongsResult()
        {
            MockSubsonicService.IsVideoPlaybackInitialized = false;
            var callCount = 0;
            MockGetRandomSongsResult mockGetRandomSongsResult = null;
            MockSubsonicService.GetRandomSongs = songCount =>
                {
                    songCount.Should().Be(1);
                    callCount++;
                    mockGetRandomSongsResult = new MockGetRandomSongsResult(songCount);
                    return mockGetRandomSongsResult;
                };

            ((IPlayer)Subject).Play(new PlaylistItem { Uri = _defaultUri });

            callCount.Should().Be(1);
            mockGetRandomSongsResult.ExecuteCallCount.Should().Be(1);
            mockGetRandomSongsResult.NumberOfSongs.Should().Be(1);
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            base.TestInitializeExtensions();
            _mockToastNotificationService = new MockToastNotificationService();
            Subject.ToastNotificationService = _mockToastNotificationService;
        }

        #endregion
    }
}