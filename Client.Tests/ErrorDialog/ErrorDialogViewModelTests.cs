namespace Client.Tests.ErrorDialog
{
    using System;
    using System.Threading.Tasks;
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

        private MockDialogNotificationService _mockDialogNotificationService;

        private MockResourceService _mockResourceService;

        private MockDialogService _mockDialogService;

        [TestInitialize]
        public void Setup()
        {
            _mockWinRTWrapperService = new MockWinRTWrappersService();
            _mockNavigationService = new MockNavigationService();
            _mockDialogNotificationService = new MockDialogNotificationService();
            _mockResourceService = new MockResourceService();
            _mockDialogService = new MockDialogService();
            _subject = new ErrorDialogViewModel(
                _mockWinRTWrapperService,
                _mockNavigationService,
                _mockDialogNotificationService,
                _mockResourceService,
                _mockDialogService);
        }

        [TestMethod]
        public void Ctor_Should_CallWinRTWrapperServiceRegisterShareRequestHandler()
        {
            _mockWinRTWrapperService.RegisterShareRequestHandlerCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task HandleError_WithString_ShouldSetIsHiddenFalse()
        {
            await _subject.HandleError(new Exception("test"));

            _subject.IsHidden.Should().BeFalse();
        }

        [TestMethod]
        public async Task HandleError_WithString_ShouldSetStringAsErrorDescription()
        {
            await _subject.HandleError(new Exception("test"));

            _subject.ErrorDescription.Should().Be("test");
        }

        [TestMethod]
        public async Task HandleError_WithException_ShouldSetExceptionAsExceptionString()
        {
            await _subject.HandleError(new Exception("test"));

            _subject.ExceptionString.Should().Be("System.Exception: test");
        }

        [TestMethod]
        public async Task HandleError_WithException_ShouldSetIsHiddenFalse()
        {
            await _subject.HandleError(new Exception("test"));

            _subject.IsHidden.Should().BeFalse();
        }

        [TestMethod]
        public async Task HandleCriticalError_Always_ShouldSetCanGoBackFalse()
        {
            await _subject.HandleCriticalError(new Exception("test"));

            _subject.CanGoBack.Should().BeFalse();
        }

        [TestMethod]
        public async Task GoBack_CanGoBack_ShouldSetIsHiddenTrue()
        {
            await _subject.HandleError(new Exception("test"));
            _mockNavigationService.CanGoBack = true;

            _subject.GoBack();

            _subject.IsHidden.Should().BeTrue();
        }

        [TestMethod]
        public void ShareErrorDetails_Always_CallsShareServiceShowUi()
        {
            _subject.ShareErrorDetails();

            _mockWinRTWrapperService.ShowShareUICallCount.Should().Be(1);
        }
    }
}