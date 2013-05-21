namespace Client.Tests.Framework.ViewModel
{
    using System;
    using Caliburn.Micro;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework.ViewModel;

    [TestClass]
    public abstract class ViewModelBaseTests<TViewModel> : ClientTestBase
        where TViewModel : IViewModel, new()
    {
        #region Properties

        protected MockDialogNotificationService MockDialogNotificationService { get; set; }

        protected MockEventAggregator MockEventAggregator { get; set; }

        protected MockNavigationService MockNavigationService { get; set; }

        protected MockSubsonicService MockSubsonicService { get; set; }

        protected abstract TViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

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
        public void OnActivateShouldSetDiplayName()
        {
            Subject.UpdateDisplayName = () => Subject.DisplayName = "42";

            ((IActivate)Subject).Activate();

            Subject.DisplayName.Should().Be("42");
        }

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
                              UpdateDisplayName = () => Subject.DisplayName = string.Empty
                          };
            TestInitializeExtensions();
        }

        #endregion

        #region Methods

        protected virtual void TestInitializeExtensions()
        {
        }

        #endregion
    }
}