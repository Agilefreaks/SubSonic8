using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Main;

namespace Client.Tests.Main
{
    [TestClass]
    public class MainViewModelTests : ViewModelBaseTests<MainViewModel>
    {
        private MockGetRootResult _mockGetRootResult;

        protected override MainViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            _mockGetRootResult = new MockGetRootResult();
            MockSubsonicService.GetRootIndex = () => _mockGetRootResult;
        }

        [TestMethod]
        public void CtorShouldInstantiateMenuItems()
        {
            Subject.MenuItems.Should().NotBeNull();
        }

        [TestMethod]
        public void SetMenuItemsShouldAddMenuItems()
        {
            Subject.SetMenuItems(new List<IndexItem> { new IndexItem(), new IndexItem() });

            Subject.MenuItems.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task PopulateWhenServiceIsConfiguredShouldExecuteAGetRootResult()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(true);

            await Task.Run(() => Subject.Populate());
            _mockGetRootResult.ExecuteCallCount.Should().Be(1);

            MockSubsonicService.SetHasValidSubsonicUrl(false);
        }

        [TestMethod]
        public async Task Populate_WhenServiceIsNotConfigured_ShouldShowANotification()
        {
            MockSubsonicService.SetHasValidSubsonicUrl(false);

            await Task.Run(() => Subject.Populate());

            MockDialogNotificationService.Showed.Count.Should().Be(1);
            const string expectedMessage =
                "You did not set up your connection. Please fill in you server address, username and password to start browsing.";
            MockDialogNotificationService.Showed[0].Message.Should().Be(expectedMessage);
        }
    }
}