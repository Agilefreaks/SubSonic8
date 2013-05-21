namespace Client.Common.Tests.Models.Subsonic
{
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    internal class SearchResultCollectionTests
    {
        #region Fields

        private SearchResultCollection _subject;

        #endregion

        #region Public Methods and Operators

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

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new SearchResultCollection();
        }

        #endregion
    }
}