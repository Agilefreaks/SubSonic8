namespace Client.Common.Tests.Results
{
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class GetIndexResultTests
    {
        #region Fields

        private GetIndexResult _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&musicFolderId=1", "you need to append the music folder id");
        }

        [TestInitialize]
        public void Setup()
        {
            _subject = new GetIndexResult(new SubsonicServiceConfiguration { BaseUrl = "http://test" }, 1);
        }

        [TestMethod]
        public void ViewName_Always_ReturnsCorrectName()
        {
            _subject.ResourcePath.Should().Be("getIndexes.view");
        }

        #endregion
    }
}