using System.Linq;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Search;
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
        public void Stop_CallsPlayerControlsStop()
        {
            Subject.Stop();

            _mockPlayerControls.StopCallCount.Should().Be(1);
        }

        [TestMethod]
        public void Handle_PerformSearh_Calls_NavigateToSearchResult()
        {
            Subject.SendSearchQueryMessage("test");

            _mockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            _mockNavigationService.NavigateToViewModelCalls.First().Key.Should().Be(typeof(SearchViewModel));
            _mockNavigationService.NavigateToViewModelCalls.First().Value.Should().BeOfType<string>();
        }
    }
}