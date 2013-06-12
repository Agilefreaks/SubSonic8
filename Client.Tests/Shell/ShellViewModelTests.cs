﻿namespace Client.Tests.Shell
{
    using System.Linq;
    using Caliburn.Micro;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Search;
    using Subsonic8.Shell;

    [TestClass]
    public class ShellViewModelTests
    {
        #region Fields

        private readonly MockEventAggregator _eventAggregator = new MockEventAggregator();

        private MockDialogNotificationService _mockDialogNotificationService;

        private MockNavigationService _mockNavigationService;

        private MockPlayerControls _mockPlayerControls;

        private MockStorageService _mockStorageService;

        private MockSubsonicService _mockSubsonicService;

        private MockToastNotificationService _mockToastNotificationService;

        private MockWinRTWrappersService _mockWinRTWrappersService;

        #endregion

        #region Properties

        protected IShellViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void CtorShouldSubscribeToEventAggregator()
        {
            _eventAggregator.Subscriber.Should().Be(Subject);
        }

        [TestMethod]
        public void Handle_PerformSearh_Calls_NavigateToSearchResult()
        {
            Subject.SendSearchQueryMessage("test");

            _mockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            _mockNavigationService.NavigateToViewModelCalls.First().Key.Should().Be(typeof(SearchViewModel));
            _mockNavigationService.NavigateToViewModelCalls.First().Value.Should().BeOfType<string>();
        }

        [TestMethod]
        public void Stop_CallsPlayerControlsStop()
        {
            Subject.Stop();

            _mockPlayerControls.StopCallCount.Should().Be(1);
        }

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
            Subject = new ShellViewModel(
                _eventAggregator,
                _mockSubsonicService,
                _mockNavigationService,
                _mockToastNotificationService,
                _mockDialogNotificationService,
                _mockStorageService,
                _mockWinRTWrappersService)
                {
                    PlayerControls = _mockPlayerControls
                };
        }

        #endregion
    }
}