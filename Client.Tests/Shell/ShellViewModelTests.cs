using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
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
        private MockToastNotificationService _mockToastNotificationService;
        private MockStorageService _mockStorageService;
        private MockWinRTWrappersService _mockWinRTWrappersService;
        private MockDialogNotificationService _mockDialogNotificationService;
        private MockPlayerControls _mockPlayerControls;

        [TestInitialize]
        public void TestInitialize()
        {
            IoC.GetInstance = (type, s) => null;
            _mockSubsonicService = new MockSubsonicService();
            _mockNavigationService = new MockNavigationService();
            _mockToastNotificationService = new MockToastNotificationService();
            _mockDialogNotificationService = new MockDialogNotificationService();
            _mockStorageService = new MockStorageService();
            _mockWinRTWrappersService = new MockWinRTWrappersService();
            _mockPlayerControls = new MockPlayerControls();
            Subject = new ShellViewModel(_eventAggregator, _mockSubsonicService, _mockNavigationService,
                _mockToastNotificationService, _mockDialogNotificationService, _mockStorageService, _mockWinRTWrappersService)
                {
                    PlayerControls = _mockPlayerControls
                };
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

        [TestMethod]
        public void Handle_WithStopAudioPlaybackMessage_CallsPlayerControlsStop()
        {
            Subject.Handle(new StopAudioPlaybackMessage());

            _mockPlayerControls.StopCallCount.Should().Be(1);
        }
    }
}