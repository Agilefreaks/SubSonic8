namespace Client.Tests.ErrorDialog
{
    using System;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.ErrorDialog;

    [TestClass]
    public class ErrorDialogViewModelTests
    {
        private ErrorDialogViewModel _subject;

        private MockWinRTWrappersService _mockWinRTWrapperService;

        [TestInitialize]
        public void Setup()
        {
            _mockWinRTWrapperService = new MockWinRTWrappersService();
            _subject = new ErrorDialogViewModel(_mockWinRTWrapperService);
        }

        [TestMethod]
        public void Ctor_Should_CallWinRTWrapperServiceRegisterShareRequestHandler()
        {
            _mockWinRTWrapperService.RegisterShareRequestHandlerCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleError_WithString_ShouldSetIsOpenTrue()
        {
            _subject.HandleError("test");

            _subject.IsOpen.Should().BeTrue();
        }

        [TestMethod]
        public void HandleError_WithString_ShouldSetStringAsErrorMessage()
        {
            _subject.HandleError("test");

            _subject.ErrorMessage.Should().Be("test");
        }

        [TestMethod]
        public void HandleError_WithException_ShouldSetExceptionAsErrorMessage()
        {
            _subject.HandleError(new Exception("test"));

            _subject.ErrorMessage.Should().Be("System.Exception: test");
        }

        [TestMethod]
        public void HandleError_WithException_ShouldSetIsOpenTrue()
        {
            _subject.HandleError(new Exception("test"));

            _subject.IsOpen.Should().BeTrue();
        }

        [TestMethod]
        public void CloseDialog_Always_ShouldSetIsOpenFalse()
        {
            _subject.HandleError("test");

            _subject.CloseDialog();

            _subject.IsOpen.Should().BeFalse();
        }

        [TestMethod]
        public void CloseDialog_Always_ShouldClearTheErrorMessage()
        {
            _subject.HandleError("test");

            _subject.CloseDialog();

            _subject.ErrorMessage.Should().BeNullOrEmpty();
        }

        [TestMethod]
        public void ShareErrorDetails_Always_ShouldSetIsOpenFalse()
        {
            _subject.HandleError("test");

            _subject.ShareErrorDetails();

            _subject.IsOpen.Should().BeFalse();
        }

        [TestMethod]
        public void ShareErrorDetails_Always_CallsShareServiceShowUi()
        {
            _subject.ShareErrorDetails();

            _mockWinRTWrapperService.ShowShareUICallCount.Should().Be(1);
        }
    }
}