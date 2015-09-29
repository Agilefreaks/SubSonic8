namespace Client.Tests.BottomBar
{
    using Client.Common.EventAggregatorMessages;
    using Client.Tests.Mocks;
    using global::Common.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.BottomBar;
    using Subsonic8.Main;

    [TestClass]
    public abstract class BottomBarViewModelTests<TViewModel>
        where TViewModel : BottomBarViewModelBase, new()
    {
        public MockNavigationService MockNavigationService { get; set; }

        public MockEventAggregator MockEventAggregator { get; set; }

        public MockPlyalistManagementService MockPlyalistManagementService { get; set; }

        public MockErrorDialogViewModel MockErrorDialogViewModel { get; set; }

        protected TViewModel Subject { get; set; }

        [TestInitialize]
        public void Setup()
        {
            MockEventAggregator = new MockEventAggregator();
            MockNavigationService = new MockNavigationService();
            MockPlyalistManagementService = new MockPlyalistManagementService();
            MockErrorDialogViewModel = new MockErrorDialogViewModel();
            Subject = new TViewModel
                           {
                               NavigationService = MockNavigationService,
                               EventAggregator = MockEventAggregator,
                               PlaylistManagementService = MockPlyalistManagementService,
                               ErrorDialogViewModel = MockErrorDialogViewModel
                           };
            TestInitializeExtensions();
        }

        [TestMethod]
        public void IsOpened_SelectedItemsCountIs1_IsTrue()
        {
            Subject.SelectedItems.Add(new object());

            Subject.IsOpened.Should().BeTrue();
        }

        [TestMethod]
        public void IsOpened_SelectedItemsBecomes0_IsFalse()
        {
            var item = new object();
            Subject.SelectedItems.Add(item);
            Subject.SelectedItems.Remove(item);

            Subject.IsOpened.Should().BeFalse();
        }

        [TestMethod]
        public void CanDismis_SelectedItemsBecomes1_IsTrue()
        {
            Subject.SelectedItems.Add(new object());

            Subject.CanDismiss.Should().BeTrue();
        }

        [TestMethod]
        public void CanDismis_SelectedItemsBecomes0_IsFalse()
        {
            var item = new object();
            Subject.SelectedItems.Add(item);
            Subject.SelectedItems.Remove(item);

            Subject.CanDismiss.Should().BeFalse();
        }

        [TestMethod]
        public void SelectionExists_SelectedItemsBecomes1_IsTrue()
        {
            Subject.SelectedItems.Add(new object());

            Subject.SelectionExists.Should().BeTrue();
        }

        [TestMethod]
        public void SelectionExists_SelectedItemsBecomes0_IsFalse()
        {
            var item = new object();
            Subject.SelectedItems.Add(item);
            Subject.SelectedItems.Remove(item);

            Subject.SelectionExists.Should().BeFalse();
        }

        [TestMethod]
        public void ClearSelection_Always_SetsCanDismisTrue()
        {
            Subject.ClearSelection();

            Subject.CanDismiss.Should().BeFalse();
        }

        [TestMethod]
        public void ClearSelection_Always_ClearsSelectedItems()
        {
            Subject.SelectedItems.Add(new object());

            Subject.ClearSelection();

            Subject.CanDismiss.Should().BeFalse();
        }

        [TestMethod]
        public void ClearSelection_Always_SetsIsOpenedFalse()
        {
            Subject.ClearSelection();

            Subject.IsOpened.Should().BeFalse();
        }

        [TestMethod]
        public void ToggleShuffle_Always_PublishesANewToggleShuffleMessage()
        {
            Subject.ToggleShuffle();

            MockEventAggregator.PublishCallCount.Should().Be(1);
            MockEventAggregator.Messages[0].Should().BeOfType<ToggleShuffleMessage>();
        }

        [TestMethod]
        public void PlayNext_Always_CallsEventAggregatorPublishJumpToNextMessage()
        {
            Subject.PlayNext();

            MockEventAggregator.PublishCallCount.Should().Be(1);
            MockEventAggregator.Messages[0].Should().BeOfType<JumpToNextMessage>();
        }

        [TestMethod]
        public void PlayPause_Always_CallsEventAggregatorPublishPlayPauseMessage()
        {
            Subject.PlayPause();

            MockEventAggregator.PublishCallCount.Should().Be(1);
            MockEventAggregator.Messages[0].Should().BeOfType<PlayPauseMessage>();
        }

        [TestMethod]
        public void PlayPrevious_Always_CallsEventAggregatorPublishPlayPreviousMessage()
        {
            Subject.PlayPrevious();

            MockEventAggregator.PublishCallCount.Should().Be(1);
            MockEventAggregator.Messages[0].Should().BeOfType<JumpToPreviousMessage>();
        }

        [TestMethod]
        public void Stop_Always_CallsEventAggregatorPublishStopMessage()
        {
            Subject.Stop();

            MockEventAggregator.PublishCallCount.Should().Be(1);
            MockEventAggregator.Messages[0].Should().BeOfType<StopMessage>();
        }

        [TestMethod]
        public void NavigateToRoot_Always_SetsCanDismissTrue()
        {
            Subject.NavigateToRoot();

            Subject.CanDismiss.Should().BeTrue();
        }

        [TestMethod]
        public void NavigateToRoot_Always_SetsIsOpenedFalse()
        {
            Subject.NavigateToRoot();

            Subject.IsOpened.Should().BeFalse();
        }

        [TestMethod]
        public void NavigateToRoot_Always_NavigatesToMainViewModel()
        {
            Subject.NavigateToRoot();

            MockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            MockNavigationService.NavigateToViewModelCalls[0].Key.Should().Be<MainViewModel>();
        }

        protected virtual void TestInitializeExtensions()
        {
        }
    }
}