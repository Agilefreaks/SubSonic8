using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.MenuItem;

namespace Client.Tests.DefaultBottomBar
{
    [TestClass]
    public class DefaultBottomBarViewModelTests
    {
        private DefaultBottomBarViewModel _subject;
        private MockEventAggregator _eventAggregator;
        private MockNavigationService _navigationService;
        private MockPlyalistManagementService _mockPlyalistManagementService;

        [TestInitialize]
        public void TestInitialize()
        {
            IoC.GetInstance = (type, s) => null;
            _eventAggregator = new MockEventAggregator();
            _navigationService = new MockNavigationService();
            _mockPlyalistManagementService = new MockPlyalistManagementService();
            _subject = new DefaultBottomBarViewModel(_navigationService, _eventAggregator, _mockPlyalistManagementService) { NavigateOnPlay = _navigationService.DoNavigate };
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
            var showControlsMessage = new PlaylistStateChangedMessage(false);

            _subject.Handle(showControlsMessage);

            _subject.DisplayPlayControls.Should().BeFalse();
        }

        [TestMethod]
        public void HandleWithShowControlsMessage_WhenShowIsTrue_SetsDisplayPlayControlsToTrue()
        {
            var showControlsMessage = new PlaylistStateChangedMessage(true);

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
            _eventAggregator.Messages[0].Should().BeOfType<StopMessage>();
        }
    }
}
