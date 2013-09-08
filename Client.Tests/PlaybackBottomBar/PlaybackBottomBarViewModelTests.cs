namespace Client.Tests.PlaybackBottomBar
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Tests.BottomBar;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.BottomBar;

    [TestClass]
    public class PlaybackBottomBarViewModelTests : BottomBarViewModelTests<PlaybackBottomBarViewModel>
    {
        #region Public Methods and Operators

        [TestMethod]
        public void CanRemoveFromPlaylist_SelectedItemsIsNotEmpty_ReturnsTrue()
        {
            Subject.SelectedItems.Add(new PlaylistItem());
            Subject.SelectedItems.Add(new PlaylistItem());
            Subject.SelectedItems.Add(new PlaylistItem());

            Subject.CanRemoveFromPlaylist.Should().BeTrue();
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublish()
        {
            Subject.RemoveFromPlaylist();

            MockEventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithQueueParameterSetToSelectedItems()
        {
            Subject.SelectedItems = new ObservableCollection<object> { new PlaylistItem() };

            Subject.RemoveFromPlaylist();

            ((RemoveItemsMessage)MockEventAggregator.Messages.Last()).Queue.Should().HaveCount(1);
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithRemoveFromPlaylistMessageType()
        {
            Subject.RemoveFromPlaylist();

            MockEventAggregator.Messages.Last().GetType().Should().Be<RemoveItemsMessage>();
        }

        #endregion
    }
}