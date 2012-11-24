using Client.Common.Results;
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
            _subject = new GetAlbumResult(new SubsonicServiceConfiguration {ServiceUrl = "{0}{1}{2}"}, 12);
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
