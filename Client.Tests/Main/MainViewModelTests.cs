﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
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
        private INavigationService _navigationService;
        private MockSubsonicService _subsonicService;

        protected override IMainViewModel Subject { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _navigationService = new MockNavigationService();
            _subsonicService = new MockSubsonicService();

            Subject = new MainViewModel { NavigationService = _navigationService, SubsonicService = _subsonicService };
        }

        [TestMethod]
        public void CtorShouldInstantiateMenuItems()
        {
            Subject.MenuItems.Should().NotBeNull();
        }

        [TestMethod]
        public void CtorShouldChangeTitleOfPageToSubsonic8()
        {
            Subject.DisplayName.Should().Be("Subsonic8");
        }

        [TestMethod]
        public void SetMenuItemsShouldAddMenuItems()
        {
            var getRootResult = new GetRootResult(null)
                                    {
                                        Result = new List<IndexItem> { new IndexItem(), new IndexItem() },
                                    };

            Subject.SetMenuItems(getRootResult);

            Subject.MenuItems.Should().HaveCount(2);
        }
        
        [TestMethod]
        public async Task PopulateShouldExecuteAGetRootResult()
        {
            var mockResult = new MockGetRootResult();
            _subsonicService.GetRootIndex = () => mockResult;

            await Task.Run(() => Subject.Populate());

            mockResult.ExecuteCallCount.Should().Be(1);
        }
    }
}