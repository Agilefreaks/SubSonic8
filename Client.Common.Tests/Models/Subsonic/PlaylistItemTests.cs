using Client.Common.Models;
using Client.Common.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Models.Subsonic
{
    [TestClass]
    public class PlaylistItemTests
    {
        PlaylistItem _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new PlaylistItem();
        }

        [TestMethod]
        public void OriginalCoverArtUrl_CoverArtUrlIsRemoteUrlAndHasASizeAttribute_ReturnsTheUrlWithTheSizeAttribute500()
        {
            _subject.CoverArtUrl = "http://asda.com?as=1&size=70";

            _subject.OriginalCoverArtUrl.Should().Be("http://asda.com?as=1&size=500");
        }

        [TestMethod]
        public void OriginalCoverArtUrl_CoverArtUrlIsRemoteUrlAndDoesNotHaveASizeAttribute_ReturnsTheUrlWithTheSizeAttribute500()
        {
            _subject.CoverArtUrl = "https://asda.com?as=1";

            _subject.OriginalCoverArtUrl.Should().Be("https://asda.com?as=1&size=500");
        }

        [TestMethod]
        public void OriginalCoverArtUrl_CoverArtUrlIsLocalFile_ReturnsTheLocalFileUrl()
        {
            _subject.CoverArtUrl = SubsonicService.CoverArtPlaceholder;

            _subject.OriginalCoverArtUrl.Should().Be(SubsonicService.CoverArtPlaceholder);
        }

        [TestMethod]
        public void Ctor_Always_SetsPlayingStateToNotPlaying()
        {
            var playlistItem = new PlaylistItem();

            playlistItem.PlayingState.Should().Be(PlaylistItemState.NotPlaying);
        }

        [TestMethod]
        public void Ctor_Always_SetsCoverArtUrlToPlaceholder()
        {
            var playlistItem = new PlaylistItem();

            playlistItem.CoverArtUrl.Should().Be(SubsonicService.CoverArtPlaceholder);
        }
    }
}
