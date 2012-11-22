using Client.Common.Models.Subsonic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Models.Subsonic
{
    [TestClass]
    class SearchResultCollectionTests
    {
        private SearchResultCollection _subject;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new SearchResultCollection();
        }

        [TestMethod]
        public void CtorAlwaysInitializesAlbumsProperty()
        {
            _subject.Albums.Should().NotBeNull();
        }

        [TestMethod]
        public void CtorAlwaysInitializesArtistsProperty()
        {
            _subject.Artists.Should().NotBeNull();
        }

        [TestMethod]
        public void CtorAlwaysInitializesSongsProperty()
        {
            _subject.Songs.Should().NotBeNull();
        }
    }
}
