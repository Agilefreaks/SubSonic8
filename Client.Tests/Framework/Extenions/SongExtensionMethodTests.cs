namespace Client.Tests.Framework.Extenions
{
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework.Extensions;

    [TestClass]
    public class SongExtensionMethodTests
    {
        #region Fields

        private MockSubsonicService _mockSubsonicService;

        private Song _song;

        private PlaylistItem _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void AsPlaylistItem_Always_CallsGetCoverArtForId()
        {
            _mockSubsonicService.GetCoverArtForIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void AsPlaylistItem_Always_SetsArtist()
        {
            _subject.Artist.Should().Be("testArtist");
        }

        [TestMethod]
        public void AsPlaylistItem_Always_SetsTheCoverArtUrlPropertyToTheResultOfGetCoverArtForId()
        {
            _subject.CoverArtUrl.Should().Be("http://test.mock");
        }

        [TestMethod]
        public void AsPlaylistItem_Always_SetsTheDurationPropertyToDuration()
        {
            _subject.PlayingState.Should().Be(PlaylistItemState.NotPlaying);
        }

        [TestMethod]
        public void AsPlaylistItem_Always_SetsThePlayingStatePropertyToNotPlaying()
        {
            _subject.PlayingState.Should().Be(PlaylistItemState.NotPlaying);
        }

        [TestMethod]
        public void AsPlaylistItem_Always_SetsTitle()
        {
            _subject.Title.Should().Be("testTitle");
        }

        [TestMethod]
        public void AsPlaylistItem_ItemIsNotVideo_SetsTypeAudio()
        {
            _song.IsVideo = false;
            var subject = _song.AsPlaylistItem(_mockSubsonicService);

            subject.Type.Should().Be(PlaylistItemTypeEnum.Audio);
        }

        [TestMethod]
        public void AsPlaylistItem_ItemIsVideo_CallsSubsonicServiceGetUriForVideoWithId()
        {
            _mockSubsonicService.GetUriForVideoWithIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void AsPlaylistItem_ItemIsVideo_SetsTheUriPropertyToTheResultOfGetUriForVideoWithId()
        {
            _subject.Uri.ToString().Should().Be("http://test.mock/");
        }

        [TestMethod]
        public void AsPlaylistItem_ItemIsVideo_SetsTypeVideo()
        {
            _subject.Type.Should().Be(PlaylistItemTypeEnum.Video);
        }

        [TestInitialize]
        public void Setup()
        {
            _song = new Song
                        {
                            Artist = "testArtist", 
                            Name = "testTitle", 
                            Id = 121, 
                            CoverArt = "test123", 
                            Duration = 123, 
                            IsVideo = true
                        };
            _mockSubsonicService = new MockSubsonicService();
            _subject = _song.AsPlaylistItem(_mockSubsonicService);
        }

        #endregion
    }
}