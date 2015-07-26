namespace Client.Tests.Shell
{
    using System.Linq;
    using Caliburn.Micro;
    using Client.Tests.Mocks;
    using global::Common.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Search;
    using Subsonic8.Shell;

    [TestClass]
    public class ShellViewModelTests
    {
        #region Fields

        private MockEventAggregator _mockEventAggregator;

        private MockDialogNotificationService _mockDialogNotificationService;

        private MockNavigationService _mockNavigationService;

        private MockStorageService _mockStorageService;

        private MockSubsonicService _mockSubsonicService;

        private MockToastNotificationService _mockToastNotificationService;

        private MockWinRTWrappersService _mockWinRTWrappersService;

        private MockErrorDialogViewModel _mockErrorDialogViewModel;

        #endregion

        #region Properties

        protected IShellViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void ConstructorShouldSubscribeToEventAggregator()
        {
            _mockEventAggregator.Subscriber.Should().Be(Subject);
        }

        [TestMethod]
        public void Handle_PerformSearch_Calls_NavigateToSearchResult()
        {
            Subject.SendSearchQueryMessage("test");

            _mockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            _mockNavigationService.NavigateToViewModelCalls.First().Key.Should().Be(typeof(SearchViewModel));
            _mockNavigationService.NavigateToViewModelCalls.First().Value.Should().BeOfType<string>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            IoC.GetInstance = (type, s) => null;
            _mockEventAggregator = new MockEventAggregator();
            _mockSubsonicService = new MockSubsonicService();
            _mockNavigationService = new MockNavigationService();
            _mockToastNotificationService = new MockToastNotificationService();
            _mockDialogNotificationService = new MockDialogNotificationService();
            _mockStorageService = new MockStorageService();
            _mockWinRTWrappersService = new MockWinRTWrappersService();
            _mockErrorDialogViewModel = new MockErrorDialogViewModel();
            Subject = new ShellViewModel
                          {
                              EventAggregator = _mockEventAggregator,
                              SubsonicService = _mockSubsonicService,
                              NavigationService = _mockNavigationService,
                              NotificationService = _mockToastNotificationService,
                              DialogNotificationService = _mockDialogNotificationService,
                              StorageService = _mockStorageService,
                              WinRTWrappersService = _mockWinRTWrappersService,
                              ErrorDialogViewModel = _mockErrorDialogViewModel
                          };
        }

        #endregion
    }
}