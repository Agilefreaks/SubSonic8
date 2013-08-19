namespace Client.Tests.Framework
{
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Tests.Mocks;
    using global::Common.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework;

    [TestClass]
    public class NotificationHelperTests
    {
        #region Fields

        private MockTileNotificationService _mockTileNotificationService;

        private MockToastNotificationService _mockToastNotificationService;

        private NotificationsHelper _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_CallsTileNotificationManagerShow()
        {
            _subject.Handle(new StartPlaybackMessage(new PlaylistItem()));

            _mockTileNotificationService.ShowCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_CallsToastNotificationManagerShow()
        {
            _subject.Handle(new StartPlaybackMessage(new PlaylistItem()));

            _mockToastNotificationService.ShowCallCount.Should().Be(1);
        }

        [TestInitialize]
        public void Setup()
        {
            _mockToastNotificationService = new MockToastNotificationService();
            _mockTileNotificationService = new MockTileNotificationService();
            _subject = new NotificationsHelper
                           {
                               EventAggregator = new MockEventAggregator(), 
                               TileNotificationService = _mockTileNotificationService, 
                               ToastNotificationService = _mockToastNotificationService
                           };
        }

        #endregion
    }
}