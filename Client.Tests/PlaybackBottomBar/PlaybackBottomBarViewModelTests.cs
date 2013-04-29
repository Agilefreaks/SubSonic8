using System.Collections.ObjectModel;
using System.Linq;
using Client.Common.EventAggregatorMessages;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;

namespace Client.Tests.PlaybackBottomBar
{
    [TestClass]
    public class PlaybackBottomBarViewModelTests
    {
        PlaybackBottomBarViewModel _subject;
        private MockNavigationService _mockNavigationService;
        private MockEventAggregator _mockEventAggregator;
        private MockPlyalistManagementService _playlistManagementService;

        [TestInitialize]
        public void Setup()
        {
            _mockNavigationService = new MockNavigationService();
            _mockEventAggregator = new MockEventAggregator();
            _playlistManagementService = new MockPlyalistManagementService();
            _subject = new PlaybackBottomBarViewModel(_mockNavigationService, _mockEventAggregator, _playlistManagementService);
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublish()
        {
            _subject.RemoveFromPlaylist();

            _mockEventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithRemoveFromPlaylistMessageType()
        {
            _subject.RemoveFromPlaylist();

            _mockEventAggregator.Messages.Last().GetType().Should().Be<RemoveItemsMessage>();
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithQueueParameterSetToSelectedItems()
        {
            _subject.SelectedItems = new ObservableCollection<object> { new Common.Models.PlaylistItem() };

            _subject.RemoveFromPlaylist();

            ((RemoveItemsMessage)_mockEventAggregator.Messages.Last()).Queue.Should().HaveCount(1);
        }

        [TestMethod]
        public void CanRemoveFromPlaylist_SelectedItemsIsNotEmpty_ReturnsTrue()
        {
            _subject.SelectedItems.Add(new Common.Models.PlaylistItem());
            _subject.SelectedItems.Add(new Common.Models.PlaylistItem());
            _subject.SelectedItems.Add(new Common.Models.PlaylistItem());

            _subject.CanRemoveFromPlaylist.Should().BeTrue();
        }
    }
}