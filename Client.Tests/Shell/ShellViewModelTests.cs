using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Services;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Shell;

namespace Client.Tests.Shell
{
    [TestClass]
    public class ShellViewModelTests
    {
        protected IShellViewModel Subject { get; set; }

        private readonly MockEventAggregator _eventAggregator = new MockEventAggregator();
        private MockSubsonicService _mockSubsonicService;
        private MockNavigationService _mockNavigationService;
        private MockNotificationService _mockNotificationService;
        private MockStorageService _mockStorageService;
        private MockWinRTWrappersService _mockWinRTWrappersService;
        private MockDialogNotificationService _mockDialogNotificationService;

        [TestInitialize]
        public void TestInitialize()
        {
            IoC.GetInstance = (type, s) => null;
            _mockSubsonicService = new MockSubsonicService();
            _mockNavigationService = new MockNavigationService();
            _mockNotificationService = new MockNotificationService();
            _mockDialogNotificationService = new MockDialogNotificationService();
            _mockStorageService = new MockStorageService();
            _mockWinRTWrappersService = new MockWinRTWrappersService();
            Subject = new ShellViewModel(_eventAggregator, _mockSubsonicService, _mockNavigationService,
                _mockNotificationService, _mockDialogNotificationService, _mockStorageService, _mockWinRTWrappersService);
        }

        [TestMethod]
        public void CtorShouldSubscribeToEventAggregator()
        {
            _eventAggregator.Subscriber.Should().Be(Subject);
        }

        [TestMethod]
        public async Task PerformSubsonicSearch_Always_CallsSubsonicServiceSearchAndReturnsTheResult()
        {
            var callCount = 0;
            var searchResult = new MockSearchResult();
            Subject.NavigateToSearhResult = collection => { };
            _mockSubsonicService.Search = s =>
                                              {
                                                  Assert.AreEqual("test", s);
                                                  callCount++;
                                                  return searchResult;
                                              };

            await Subject.PerformSubsonicSearch("test");

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PerformSubsonicSearchAlwaysCallsNavigatesToSearchResult()
        {
            var called = false;
            var searchResult = new MockSearchResult();
            _mockSubsonicService.Search = s => searchResult;
            Subject.NavigateToSearhResult = collection => { called = true; };

            await Subject.PerformSubsonicSearch("test");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void PlayNextShouldCallPublishOnEventAggregator()
        {
            Subject.PlayNext(null, null);
            _eventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public void PlayPreviousShouldCallPublishOnEventAggregator()
        {
            Subject.PlayPrevious(null, null);

            _eventAggregator.PublishCallCount.Should().Be(1);
        }
    }
}