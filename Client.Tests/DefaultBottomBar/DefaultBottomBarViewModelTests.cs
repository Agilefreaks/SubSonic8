using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.MenuItem;
using Subsonic8.Messages;

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
            IoC.GetInstance = (type, s) => null;
            _eventAggregator = new MockEventAggregator();
            _navigationService = new MockNavigationService();
            _subject = new DefaultBottomBarViewModel(_navigationService, _eventAggregator) { Navigate = _navigationService.DoNavigate };
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
        public void PlayAllCallShouldClearSelectedItemsCollection()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(new MenuItemViewModel());

            _subject.PlayAll();

            _subject.SelectedItems.Should().HaveCount(0);
        }

        [TestMethod]
        public void PlayAllCallsEventAggregatorPublish()
        {
            _subject.PlayAll();

            _eventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public void PlayAllCallsNavigationServiceNavigateToViewModel()
        {
            _subject.PlayAll();

            _navigationService.NavigateToViewModelCalls.Count.Should().Be(1);
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

            _eventAggregator.Messages.Last().GetType().Should().Be<RemoveItemsMessage>();
        }

        [TestMethod]
        public void RemoveFromPlaylistCallsEventAggregatorPublishWithQueueParameterSetToSelectedItems()
        {
            _subject.SelectedItems = new ObservableCollection<object> { new Common.Models.PlaylistItem() };

            _subject.RemoveFromPlaylist();

            ((RemoveItemsMessage)_eventAggregator.Messages.Last()).Queue.Should().HaveCount(1);
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

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreOfTypeMenuItemViewModel_ReturnsTrue()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(new MenuItemViewModel());

            _subject.CanAddToPlaylist.Should().BeTrue();
        }

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreNotAllOfTypeMenuItemViewModel_ReturnsFalse()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(42);
            _subject.SelectedItems.Add(new Common.Models.PlaylistItem());

            _subject.CanAddToPlaylist.Should().BeFalse();
        }

        [TestMethod]
        public void CanRemoveFromPlaylist_SelectedItemsAreOfTypePlaylistItemViewModel_ReturnsTrue()
        {
            _subject.SelectedItems.Add(new Common.Models.PlaylistItem());
            _subject.SelectedItems.Add(new Common.Models.PlaylistItem());
            _subject.SelectedItems.Add(new Common.Models.PlaylistItem());

            _subject.CanRemoveFromPlaylist.Should().BeTrue();
        }

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreNotAllOfTypePlaylisttemViewModel_ReturnsFalse()
        {
            _subject.SelectedItems.Add(42);
            _subject.SelectedItems.Add(new Common.Models.PlaylistItem());
            _subject.SelectedItems.Add(new MenuItemViewModel());

            _subject.CanAddToPlaylist.Should().BeFalse();
        }

        [TestMethod]
        public void HandleWithShowControlsMessage_WhenShowIsFalse_SetsDisplayPlayControlsToFalse()
        {
            _subject.DisplayPlayControls = true;
            var showControlsMessage = new ShowControlsMessage
            {
                Show = false
            };

            _subject.Handle(showControlsMessage);

            _subject.DisplayPlayControls.Should().BeFalse();
        }

        [TestMethod]
        public void HandleWithShowControlsMessage_WhenShowIsTrue_SetsDisplayPlayControlsToTrue()
        {
            _subject.DisplayPlayControls = false;
            var showControlsMessage = new ShowControlsMessage
            {
                Show = true
            };

            _subject.Handle(showControlsMessage);

            _subject.DisplayPlayControls.Should().BeTrue();
        }

        [TestMethod]
        public void ToggleShuffle_Always_PublishesANewToggleShuffleMessage()
        {
            _subject.ToggleShuffle();

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<ToggleShuffleMessage>();
        }

        [TestMethod]
        public void Stop_Always_SendsAStopPlaybackMessage()
        {
            _subject.Stop();

            _eventAggregator.Messages.Count.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<StopPlaybackMessage>();
        }
    }
}
