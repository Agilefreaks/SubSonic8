using System;
using Caliburn.Micro;
using Client.Common.Services;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Shell;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public abstract class ViewModelBaseTests<TViewModel> : ClientTestBase
        where TViewModel : IViewModel
    {
        protected MockDefaultBottomBarViewModel MockDefaultBottomBar;
        protected MockShellViewModel MockShellViewModel;
        protected MockSubsonicService MockSubsonicService;
        protected MockNavigationService MockNavigationService;
        protected MockDialogNotificationService MockDialogNotificationService;

        protected abstract TViewModel Subject { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            MockDefaultBottomBar = new MockDefaultBottomBarViewModel();
            MockShellViewModel = new MockShellViewModel();
            MockSubsonicService = new MockSubsonicService();
            MockNavigationService = new MockNavigationService();
            MockDialogNotificationService = new MockDialogNotificationService();
            IoC.GetInstance = (type, s) => ResolveType(type);
            TestInitializeExtensions();
        }

        [TestMethod]
        public void CtorShouldSetDefaultBottomBar()
        {
            Subject.BottomBar.Should().NotBeNull();
        }

        [TestMethod]
        public void SelectedItemWhenBottomBarIsNillShouldNotThrowException()
        {
            Subject.BottomBar = null;

            Subject.SelectedItems.Should().NotBeNull();
        }

        [TestMethod]
        public void OnActivateShouldSetDiplayName()
        {
            Subject.UpdateDisplayName = () => Subject.DisplayName = "42";

            ((IActivate)Subject).Activate();

            Subject.DisplayName.Should().Be("42");
        }

        [TestMethod]
        public void HandleErrorCallsNotificationServiceShow()
        {
            var exception = new Exception("oops?");
            var mockDialogNotificationService = new MockDialogNotificationService();
            Subject.NotificationService = mockDialogNotificationService;

            Subject.HandleError(exception);

            mockDialogNotificationService.Showed.Count.Should().Be(1);
        }

        [TestMethod]
        public virtual void OnActivateShouldSetBottomBarIsOnPlaylistToFalse()
        {
            Subject.BottomBar.IsOnPlaylist = true;

            Subject.Activate();

            Subject.BottomBar.IsOnPlaylist.Should().BeFalse();
        }

        protected virtual void TestInitializeExtensions()
        {            
        }

        private object ResolveType(Type type)
        {
            var result = new object();
            if (type == typeof(IDefaultBottomBarViewModel))
            {
                result = MockDefaultBottomBar;
            }
            else if (type == typeof(IShellViewModel))
            {
                result = MockShellViewModel;
            }
            else if (type == typeof(ISubsonicService))
            {
                result = MockSubsonicService;
            }
            else if (type == typeof(INavigationService))
            {
                result = MockNavigationService;
            } 
            else if (type == typeof (IDialogNotificationService))
            {
                result = MockDialogNotificationService;
            }

            return result;
        }
    }
}