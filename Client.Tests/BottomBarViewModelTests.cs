using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.MenuItem;

namespace Client.Tests
{
    [TestClass]
    public class BottomBarViewModelTests
    {
        private BottomBarViewModel _subject;
        private MockEventAggregator _eventAggregator;

        [TestInitialize]
        public void TestInitialize()
        {
            _eventAggregator = new MockEventAggregator();
            _subject = new BottomBarViewModel(_eventAggregator);
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
    }
}
