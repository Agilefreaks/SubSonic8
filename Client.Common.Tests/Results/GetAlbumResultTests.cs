using Client.Common.Results;
using Client.Common.Services;
using Client.Common.Services.DataStructures.SubsonicService;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class GetAlbumResultTests
    {
        private IGetAlbumResult _subject;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new GetAlbumResult(new SubsonicServiceConfiguration { BaseUrl = "http://test" }, 12);
        }

        [TestMethod]
        public void ViewNameShouldBegetMusicDirectory()
        {
            _subject.ViewName.Should().Be("getAlbum.view");
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&id=12");
        }
    }
}
