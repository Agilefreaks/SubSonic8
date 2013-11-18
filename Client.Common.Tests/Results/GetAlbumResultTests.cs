namespace Client.Common.Tests.Results
{
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class GetAlbumResultTests
    {
        #region Fields

        private IGetAlbumResult _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&id=12");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new GetAlbumResult(new SubsonicServiceConfiguration { BaseUrl = "http://test" }, 12);
        }

        [TestMethod]
        public void ViewNameShouldBegetMusicDirectory()
        {
            _subject.ResourcePath.Should().Be("getAlbum.view");
        }

        #endregion
    }
}