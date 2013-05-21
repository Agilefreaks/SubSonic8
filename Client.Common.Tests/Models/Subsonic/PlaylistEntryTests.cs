namespace Client.Common.Tests.Models.Subsonic
{
    using System;
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class PlaylistEntryTests
    {
        private PlaylistEntry _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new PlaylistEntry();
        }

        [TestMethod]
        public void SettingName_Always_SetsTitle()
        {
            _subject.Name = "test_n";

            _subject.Title.Should().Be("test_n");
        }

        [TestMethod]
        public void GettingName_Always_ShouldReturnTitleValue()
        {
            _subject.Title = "test_t";

            _subject.Name.Should().Be("test_t");
        }

        [TestMethod]
        public void GetDescription_Always_ReturnsATupleWithTheNameArtistAndAlbumValues()
        {
            _subject.Name = "test_n";
            _subject.Artist = "test_a";
            _subject.Album = "test_ab";

            _subject.GetDescription().Should().Be(new Tuple<string, string>("test_n", "Artist: test_a, Album: test_ab"));
        }
    }
}