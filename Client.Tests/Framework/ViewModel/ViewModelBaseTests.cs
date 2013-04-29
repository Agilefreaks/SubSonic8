using System;
using Caliburn.Micro;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Framework.ViewModel;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public abstract class ViewModelBaseTests<TViewModel> : ClientTestBase
        where TViewModel : IViewModel, new()
    {
        protected MockSubsonicService MockSubsonicService;
        protected MockNavigationService MockNavigationService;
        protected MockDialogNotificationService MockDialogNotificationService;
        protected MockEventAggregator MockEventAggregator;

        protected abstract TViewModel Subject { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            MockSubsonicService = new MockSubsonicService();
            MockNavigationService = new MockNavigationService();
            MockDialogNotificationService = new MockDialogNotificationService();
            MockEventAggregator = new MockEventAggregator();
            Subject = new TViewModel
                {
                    EventAggregator = MockEventAggregator,
                    SubsonicService = MockSubsonicService,
                    NavigationService = MockNavigationService,
                    NotificationService = MockDialogNotificationService,
                    UpdateDisplayName = () => Subject.DisplayName = ""
                };
            TestInitializeExtensions();
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

        protected virtual void TestInitializeExtensions()
        {
        }
    }
}