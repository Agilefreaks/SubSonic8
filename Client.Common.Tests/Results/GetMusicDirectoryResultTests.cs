namespace Client.Common.Tests.Results
{
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class GetMusicDirectoryResultTests
    {
        #region Fields

        private IGetMusicDirectoryResult _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&id=42", "you need to append the id");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new GetMusicDirectoryResult(new SubsonicServiceConfiguration { BaseUrl = "http://test" }, 42);
        }

        [TestMethod]
        public void ViewNameShouldBegetMusicDirectory()
        {
            _subject.ResourcePath.Should().Be("getMusicDirectory.view");
        }

        #endregion
    }
}