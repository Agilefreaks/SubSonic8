using Client.Common.Results;
using Client.Common.Services;
using Client.Common.Services.DataStructures.SubsonicService;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class GetIndexResultTests
    {
        GetIndexResult _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new GetIndexResult(new SubsonicServiceConfiguration { BaseUrl = "http://test" }, 1);
        }

        [TestMethod]
        public void ViewName_Always_ReturnsCorrectName()
        {
            _subject.ViewName.Should().Be("getIndexes.view");
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&musicFolderId=1", "you need to append the music folder id");
        }
    }
}
