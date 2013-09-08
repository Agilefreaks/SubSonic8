namespace Client.Tests.Main
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Common.Exceptions;
    using Client.Common.Models.Subsonic;
    using Client.Tests.Framework.ViewModel;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Main;

    [TestClass]
    public class MainViewModelTests : ViewModelBaseTests<MainViewModel>
    {
        #region Fields

        private MockGetRootResult _mockGetRootResult;

        private MockResourceService _mockResourceService;

        private MockDialogService _mockDialogService;

        #endregion

        #region Properties

        protected override MainViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void CtorShouldInstantiateMenuItems()
        {
            Subject.MenuItems.Should().NotBeNull();
        }

        [TestMethod]
        public async Task PopulateWhenServiceIsConfiguredAndPingResultIsOkShouldExecuteAGetRootResult()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(true);
            MockSubsonicService.Ping = () => new MockPingResult();

            await Subject.Populate();
            _mockGetRootResult.ExecuteCallCount.Should().Be(1);

            MockSubsonicService.SetHasValidSubsonicUrl(false);
        }

        [TestMethod]
        public async Task Populate_PingResultHasError_CallsErrorDialogViewModelHandle()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(true);
            var apiException = new ApiException(new Error { Message = "test_m" });
            var mockPingResult = new MockPingResult { GetErrorFunc = () => apiException };
            MockSubsonicService.Ping = () => mockPingResult;

            await Subject.Populate();

            MockErrorDialogViewModel.HandledErrors.Count().Should().Be(1);
            MockErrorDialogViewModel.HandledErrors.First().Should().Be(apiException);
        }

        [TestMethod]
        public async Task Populate_WhenServiceIsConfigured_WillRunAPingResult()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(true);
            var mockPingResult = new MockPingResult();
            var callCount = 0;
            MockSubsonicService.Ping = () =>
                {
                    callCount++;
                    return mockPingResult;
                };

            await Subject.Populate();

            callCount.Should().Be(1);
            mockPingResult.ExecuteCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Populate_WhenServiceIsNotConfigured_ShouldShowANotification()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(false);
            const string ExpectedMessage =
                "You did not set up your connection. Please fill in you server address, username and password to start browsing.";
            _mockResourceService.GetStringResourceFunc = s => ExpectedMessage;

            await Subject.Populate();

            MockDialogNotificationService.Showed.Count.Should().Be(1);
            MockDialogNotificationService.Showed[0].Message.Should().Be(ExpectedMessage);
        }

        [TestMethod]
        public async Task Populate_WhenServiceIsNotConfigured_ShouldShowTheSettingsPanel()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(false);

            await Subject.Populate();

            _mockDialogService.ShowSettingsCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task PopulateWhenResultIsSuccessfull()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(true);
            MockSubsonicService.Ping = () => new MockPingResult();
            MockSubsonicService.GetMusicFolders =
                () =>
                new MockGetRootResult
                    {
                        GetResultFunc =
                            () => new List<MusicFolder> { new MusicFolder(), new MusicFolder() }
                    };

            await Subject.Populate();

            Subject.MenuItems.Should().HaveCount(2);
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            _mockGetRootResult = new MockGetRootResult { GetResultFunc = () => new List<MusicFolder>() };
            MockSubsonicService.GetMusicFolders = () => _mockGetRootResult;
            _mockResourceService = new MockResourceService();
            Subject.ResourceService = _mockResourceService;
            _mockDialogService = new MockDialogService();
            Subject.DialogService = _mockDialogService;
        }

        #endregion
    }
}