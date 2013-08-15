namespace Client.Tests.Main
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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

            await Task.Run(() => Subject.Populate());
            _mockGetRootResult.ExecuteCallCount.Should().Be(1);

            MockSubsonicService.SetHasValidSubsonicUrl(false);
        }

        [TestMethod]
        public async Task Populate_PingResultHasAPIError_CallsErrorDialogViewModelHandle()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(true);
            var mockPingResult = new MockPingResult { ApiError = new Error { Message = "test_m" } };
            MockSubsonicService.Ping = () => mockPingResult;

            await Task.Run(() => Subject.Populate());

            MockErrorDialogViewModel.HandleErrorCallCount.Should().Be(1);
            MockErrorDialogViewModel.HandledErrors.First().Should().Be("test_m");
        }

        [TestMethod]
        public async Task Populate_WhenServiceIsConfiguredAndParameterIsTrue_WillRunAPingResult()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(true);
            Subject.Parameter = true;
            var mockPingResult = new MockPingResult();
            var callCount = 0;
            MockSubsonicService.Ping = () =>
                {
                    callCount++;
                    return mockPingResult;
                };

            await Task.Run(() => Subject.Populate());

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

            await Task.Run(() => Subject.Populate());

            MockDialogNotificationService.Showed.Count.Should().Be(1);
            MockDialogNotificationService.Showed[0].Message.Should().Be(ExpectedMessage);
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

            await Task.Run(() => Subject.Populate());

            Subject.MenuItems.Should().HaveCount(2);
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            _mockGetRootResult = new MockGetRootResult();
            MockSubsonicService.GetMusicFolders = () => _mockGetRootResult;
            _mockResourceService = new MockResourceService();
            Subject.ResourceService = _mockResourceService;
        }

        #endregion
    }
}