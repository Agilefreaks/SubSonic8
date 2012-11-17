using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Main;
using Windows.UI.Xaml.Navigation;

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
            _subject.MenuItems.Should().BeEmpty();
        }

        [TestMethod]
        public void IndexClickShouldNavigateToViewModel()
        {
            // Can we test it?
            // _subject.IndexClick(new ItemClickEventArgs());
        }

        [TestMethod]
        public void PopulateShouldAddMenuItems()
        {
            _subsonicService.GetRootIndex = () => new GetRootResult(null) { Result = new List<IndexItem> { new IndexItem(), new IndexItem() } };

            _subject.Populate().ToList();

            _subject.MenuItems.Should().HaveCount(2);
        }

        #region Mocks

        internal class MockNavigationService : INavigationService
        {
            public void NavigateToViewModel<TViewModel>(object item)
            {
                Debugger.Break();
            }

            public bool Navigate(Type sourcePageType)
            {
                throw new NotImplementedException();
            }

            public bool Navigate(Type sourcePageType, object parameter)
            {
                throw new NotImplementedException();
            }

            public void GoForward()
            {
                throw new NotImplementedException();
            }

            public void GoBack()
            {
                throw new NotImplementedException();
            }

            public Type SourcePageType { get; set; }
            
            public Type CurrentSourcePageType { get; private set; }
            
            public bool CanGoForward { get; private set; }
            
            public bool CanGoBack { get; private set; }

            public event NavigatedEventHandler Navigated;
            
            public event NavigatingCancelEventHandler Navigating;
            
            public event NavigationFailedEventHandler NavigationFailed;
            
            public event NavigationStoppedEventHandler NavigationStopped;
        }

        #endregion
    }
}