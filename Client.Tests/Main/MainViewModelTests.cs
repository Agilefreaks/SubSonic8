using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Main;

namespace Client.Tests.Main
{
    [TestClass]
    public class MainViewModelTests
    {
        private IMainViewModel _subject;
        private INavigationService _navigationService;
        private ISubsonicService _subsonicService;

        [TestInitialize]
        public void TestInitialize()
        {
            _navigationService = new MockNavigationService();
            _subsonicService = new SubsonicService();
            _subject = new MainViewModel { NavigationService = _navigationService, SubsonicService = _subsonicService };
        }

        [TestMethod]
        public void CtorShouldInstantiateMenuItems()
        {
            _subject.MenuItems.Should().NotBeNull();
        }

        [TestMethod]
        public void IndexClickShouldNavigateToViewModel()
        {
            // Can we test it?
            // _subject.IndexClick(new ItemClickEventArgs());
        }

        [TestMethod]
        public void SetMenuItemsShouldAddMenuItems()
        {
            var getRootResult = new GetRootResult(null)
                                    {
                                        Result = new List<IndexItem> { new IndexItem(), new IndexItem() },
                                    };

            _subject.SetMenuItems(getRootResult);

            _subject.MenuItems.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task PopulateShouldExecuteAGetRootResult()
        {
            var mockResult = new MockGetRootResult();
            _subsonicService.GetRootIndex = () => mockResult;

            await Task.Run(() => _subject.Populate());

            mockResult.ExecuteCallCount.Should().Be(1);
        }
    }
}