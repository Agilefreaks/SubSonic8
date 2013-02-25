using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Models.Subsonic
{
    [TestClass]
    public class PlaylistItemTests
    {
        PlaylistItem _subject;
        private MockSubsonicService _mockSubsonicService;

        [TestInitialize]
        public void Setup()
        {
            _subject = new PlaylistItem();
            _mockSubsonicService = new MockSubsonicService();
        }

        [TestMethod]
        public void InitializeFromSong_Always_SetsArtist()
        {
            _subject.InitializeFromSong(new Song { Artist = "testArtist" }, _mockSubsonicService);

            _subject.Artist.Should().Be("testArtist");
        }

        [TestMethod]
        public void InitializeFromSong_Always_SetsTitle()
        {
            _subject.InitializeFromSong(new Song { Title = "testTitle" }, _mockSubsonicService);

            _subject.Title.Should().Be("testTitle");
        }

        [TestMethod]
        public void InitializeFromSong_ItemIsVideo_CallsSubsonicServiceGetUriForVideoWithId()
        {
            _subject.InitializeFromSong(new Song { Id = 123, IsVideo = true }, _mockSubsonicService);

            _mockSubsonicService.GetUriForVideoWithIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void InitializeFromSong_ItemIsVideo_SetsTheUriPropertyToTheResultOfGetUriForVideoWithId()
        {
            _subject.InitializeFromSong(new Song { Id = 123, IsVideo = true }, _mockSubsonicService);

            _subject.Uri.ToString().Should().Be("http://test.mock/123");
        }

        [TestMethod]
        public void InitializeFromSong_Always_CallsGetCoverArtForId()
        {
            _subject.InitializeFromSong(new Song(), _mockSubsonicService);

            _mockSubsonicService.GetCoverArtForIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void InitializeFromSong_Always_SetsTheCoverArtUrlPropertyToTheResultOfGetCoverArtForId()
        {
            _subject.InitializeFromSong(new Song { Id = 121, CoverArt = "test123" }, _mockSubsonicService);

            _subject.CoverArtUrl.Should().Be("test123");
        }

        [TestMethod]
        public void InitializeFromSong_Always_SetsThePlayingStatePropertyToNotPlaying()
        {
            _subject.InitializeFromSong(new Song(), _mockSubsonicService);

            _subject.PlayingState.Should().Be(PlaylistItemState.NotPlaying);
        }

        [TestMethod]
        public void InitializeFromSong_Always_SetsTheDurationPropertyToDuration()
        {
            _subject.InitializeFromSong(new Song { Duration = 123 }, _mockSubsonicService);

            _subject.PlayingState.Should().Be(PlaylistItemState.NotPlaying);
        }

        [TestMethod]
        public void InitializeFromSong_ItemIsVideo_SetsTypeVideo()
        {
            _subject.InitializeFromSong(new Song { IsVideo = true }, _mockSubsonicService);

            _subject.Type.Should().Be(PlaylistItemTypeEnum.Video);
        }

        [TestMethod]
        public void InitializeFromSong_ItemIsNotVideo_SetsTypeAudio()
        {
            _subject.InitializeFromSong(new Song { IsVideo = false }, _mockSubsonicService);

            _subject.Type.Should().Be(PlaylistItemTypeEnum.Audio);
        }
    }
}
