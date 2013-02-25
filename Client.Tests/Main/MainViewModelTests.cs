using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Main;

namespace Client.Tests.Main
{
    [TestClass]
    public class MainViewModelTests : ViewModelBaseTests<IMainViewModel>
    {
        private MockGetRootResult _mockGetRootResult;

        protected override IMainViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            _mockGetRootResult = new MockGetRootResult();
            MockSubsonicService.GetRootIndex = () => _mockGetRootResult;

            Subject = new MainViewModel
                          {
                              UpdateDisplayName = () => Subject.DisplayName = ""
                          };
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
            MockSubsonicService.SetIsConfigured(true);


            await Task.Run(() => Subject.Populate());
            _mockGetRootResult.ExecuteCallCount.Should().Be(1);

            MockSubsonicService.SetIsConfigured(false);
        }
    }
}