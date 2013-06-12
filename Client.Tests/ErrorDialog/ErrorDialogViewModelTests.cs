namespace Client.Tests.ErrorDialog
{
    using System;
    using System.Linq;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.ErrorDialog;

    [TestClass]
    public class ErrorDialogViewModelTests
    {
        private ErrorDialogViewModel _subject;

        private MockWinRTWrappersService _mockWinRTWrapperService;

        private MockNavigationService _mockNavigationService;

        [TestInitialize]
        public void Setup()
        {
            _mockWinRTWrapperService = new MockWinRTWrappersService();
            _mockNavigationService = new MockNavigationService();
            _subject = new ErrorDialogViewModel(_mockWinRTWrapperService, _mockNavigationService)
                           {
                               NavigateAction = _mockNavigationService.DoNavigate
                           };
        }

        [TestMethod]
        public void Ctor_Should_CallWinRTWrapperServiceRegisterShareRequestHandler()
        {
            _mockWinRTWrapperService.RegisterShareRequestHandlerCallCount.Should().Be(1);
        }

        [TestMethod]
        public void Ctor_Always_ShouldSetTheNavigateActionToNavigate()
        {
            var errorDialogViewModel = new ErrorDialogViewModel(_mockWinRTWrapperService, _mockNavigationService);

            errorDialogViewModel.NavigateAction.Should().Be((Action<Type>)errorDialogViewModel.Navigate);
        }

        [TestMethod]
        public void HandleError_WithString_ShouldNavigateToErrorDialogViewModel()
        {
            _subject.HandleError("test");

            _mockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            _mockNavigationService.NavigateToViewModelCalls.First().Key.Should().Be(typeof(ErrorDialogViewModel));
        }

        [TestMethod]
        public void HandleError_WithString_ShouldSetStringAsErrorDescription()
        {
            _subject.HandleError("test");

            _subject.ErrorDescription.Should().Be("test");
        }

        [TestMethod]
        public void HandleError_WithException_ShouldSetExceptionAsExceptionString()
        {
            _subject.HandleError(new Exception("test"));

            _subject.ExceptionString.Should().Be("System.Exception: test");
        }

        [TestMethod]
        public void HandleError_WithException_ShouldNavigateToSelf()
        {
            _subject.HandleError(new Exception("test"));

            _mockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            _mockNavigationService.NavigateToViewModelCalls.First().Key.Should().Be(typeof(ErrorDialogViewModel));
        }

        [TestMethod]
        public void GoBack_CanGoBack_ShouldSetIsOpenFalse()
        {
            _subject.HandleError("test");
            _mockNavigationService.CanGoBack = true;

            _subject.GoBack();

            _mockNavigationService.GoBackCallCount.Should().Be(1);
        }

        [TestMethod]
        public void ShareErrorDetails_Always_CallsShareServiceShowUi()
        {
            _subject.ShareErrorDetails();

            _mockWinRTWrapperService.ShowShareUICallCount.Should().Be(1);
        }
    }
}