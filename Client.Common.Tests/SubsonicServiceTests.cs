using Client.Common.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests
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
            _subject.Search.Should().NotBeNull();
        }
    }
}
