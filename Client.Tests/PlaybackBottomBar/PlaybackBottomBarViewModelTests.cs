namespace Client.Tests.PlaybackBottomBar
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.BottomBar;

    [TestClass]
    public class PlaybackBottomBarViewModelTests
    {
        #region Fields

        private MockEventAggregator _mockEventAggregator;

        private MockNavigationService _mockNavigationService;

        private MockPlyalistManagementService _playlistManagementService;

        private PlaybackBottomBarViewModel _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void CanRemoveFromPlaylist_SelectedItemsIsNotEmpty_ReturnsTrue()
        {
            _subject.SelectedItems.Add(new PlaylistItem());
            _subject.SelectedItems.Add(new PlaylistItem());
            _subject.SelectedItems.Add(new PlaylistItem());

            _subject.CanRemoveFromPlaylist.Should().BeTrue();
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublish()
        {
            _subject.RemoveFromPlaylist();

            _mockEventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithQueueParameterSetToSelectedItems()
        {
            _subject.SelectedItems = new ObservableCollection<object> { new PlaylistItem() };

            _subject.RemoveFromPlaylist();

            ((RemoveItemsMessage)_mockEventAggregator.Messages.Last()).Queue.Should().HaveCount(1);
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithRemoveFromPlaylistMessageType()
        {
            _subject.RemoveFromPlaylist();

            _mockEventAggregator.Messages.Last().GetType().Should().Be<RemoveItemsMessage>();
        }

        [TestInitialize]
        public void Setup()
        {
            _mockNavigationService = new MockNavigationService();
            _mockEventAggregator = new MockEventAggregator();
            _playlistManagementService = new MockPlyalistManagementService();
            _subject = new PlaybackBottomBarViewModel(
                _mockNavigationService, _mockEventAggregator, _playlistManagementService);
        }

        #endregion
    }
}