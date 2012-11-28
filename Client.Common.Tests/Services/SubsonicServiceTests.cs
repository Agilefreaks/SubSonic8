using Client.Common.Results;
using Client.Common.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Services
{
    [TestClass]
    public class SubsonicServiceTests
    {
        private SubsonicService _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new SubsonicService();
        }

        [TestMethod]
        public void GetRootIndexAlwaysReturnsAGetIndexResult()
        {
            var result = _subject.GetRootIndex();

            Assert.IsInstanceOfType(result, typeof(GetRootResult));
        }

        [TestMethod]
        public void CtorShouldFunctions()
        {
            _subject.GetMusicDirectory.Should().NotBeNull();
            _subject.GetRootIndex.Should().NotBeNull();
            _subject.GetAlbum.Should().NotBeNull();
            _subject.Search.Should().NotBeNull();
        }
    }
}
