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

        private MockSharingService _mockSharingService;

        [TestInitialize]
        public void Setup()
        {
            _mockSharingService = new MockSharingService();
            _subject = new ErrorDialogViewModel
                           {
                               SharingService = _mockSharingService
                           };
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

            _mockSharingService.ShowShareUICallCount.Should().Be(1);
        }
    }
}