using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.MenuItem;
using Subsonic8.Messages;
using Subsonic8.PlaylistItem;

namespace Client.Tests.DefaultBottomBar
{
    [TestClass]
    public class DefaultBottomBarViewModelTests
    {
        private DefaultBottomBarViewModel _subject;
        private MockEventAggregator _eventAggregator;
        private MockNavigationService _navigationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _eventAggregator = new MockEventAggregator();
            _navigationService = new MockNavigationService();
            _subject = new DefaultBottomBarViewModel(_navigationService, _eventAggregator);
        }

        [TestMethod]
        public void CtorShouldInitializeSelectedItemsCollection()
        {
            _subject.SelectedItems.Should().NotBeNull();
        }

        [TestMethod]
        public void AddToPlaylistCallsEventAggregatorPublish()
        {
            _subject.AddToPlaylist();

            _eventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public void AddToPlaylistCallShouldClearSelectedItemsCollection()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(new MenuItemViewModel());

            _subject.AddToPlaylist();

            _subject.SelectedItems.Should().HaveCount(0);
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublish()
        {
            _subject.RemoveFromPlaylist();

            _eventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithRemoveFromPlaylistMessageType()
        {
            _subject.RemoveFromPlaylist();

            _eventAggregator.Messages.Last().GetType().Should().Be<RemoveFromPlaylistMessage>();
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithQueueParameterSetToSelectedItems()
        {
            _subject.SelectedItems = new ObservableCollection<object> { new PlaylistItemViewModel() };
            
            _subject.RemoveFromPlaylist();

            ((RemoveFromPlaylistMessage)_eventAggregator.Messages.Last()).Queue.Should().HaveCount(1);
        }

        [TestMethod]
        public void IsOpened_SelectedItemsIsEmpty_ReturnsFalse()
        {
            _subject.SelectedItems = new ObservableCollection<object>();

            var isOpened = _subject.IsOpened;

            isOpened.Should().BeFalse();
        }

        [TestMethod]
        public void IsOpened_SelectedItemsIsNotEmpty_ReturnsTrue()
        {
            _subject.SelectedItems = new ObservableCollection<object> { new MenuItemViewModel() };

            var isOpened = _subject.IsOpened;

            isOpened.Should().BeTrue();
        }
    }
}
