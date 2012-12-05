﻿using System.Collections.ObjectModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.MenuItem;
using Subsonic8.Playback;

namespace Client.Tests
{
    [TestClass]
    public class MediaSelectionBottomBarViewModelTests
    {
        private MediaSelectionBottomBarViewModel _subject;
        private MockEventAggregator _eventAggregator;
        private MockNavigationService _navigationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _eventAggregator = new MockEventAggregator();
            _navigationService = new MockNavigationService();
            _subject = new MediaSelectionBottomBarViewModel(_navigationService, _eventAggregator);
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
        public void IsOpened_SelectedItemsIsEmpty_ReturnsFalse()
        {
            _subject.SelectedItems = new ObservableCollection<MenuItemViewModel>();

            var isOpened = _subject.IsOpened;

            isOpened.Should().BeFalse();
        }

        [TestMethod]
        public void IsOpened_SelectedItemsIsNotEmpty_ReturnsTrue()
        {
            _subject.SelectedItems = new ObservableCollection<MenuItemViewModel> { new MenuItemViewModel() };

            var isOpened = _subject.IsOpened;

            isOpened.Should().BeTrue();
        }
    }
}
