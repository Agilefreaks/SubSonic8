namespace Client.Common.Tests.Models.Subsonic
{
    using Client.Common.Models;
    using Client.Common.Services;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class PlaylistItemTests
    {
        #region Fields

        private PlaylistItem _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void Ctor_Always_SetsCoverArtUrlToPlaceholder()
        {
            var playlistItem = new PlaylistItem();

            playlistItem.CoverArtUrl.Should().Be(SubsonicService.CoverArtPlaceholder);
        }

        [TestMethod]
        public void Ctor_Always_SetsPlayingStateToNotPlaying()
        {
            var playlistItem = new PlaylistItem();

            playlistItem.PlayingState.Should().Be(PlaylistItemState.NotPlaying);
        }

        [TestMethod]
        public void OriginalCoverArtUrl_CoverArtUrlIsLocalFile_ReturnsTheLocalFileUrl()
        {
            _subject.CoverArtUrl = SubsonicService.CoverArtPlaceholder;

            _subject.OriginalCoverArtUrl.Should().Be(SubsonicService.CoverArtPlaceholder);
        }

        [TestMethod]
        public void
            OriginalCoverArtUrl_CoverArtUrlIsRemoteUrlAndDoesNotHaveASizeAttribute_ReturnsTheUrlWithTheSizeAttribute500()
        {
            _subject.CoverArtUrl = "https://asda.com?as=1";

            _subject.OriginalCoverArtUrl.Should().Be("https://asda.com?as=1&size=500");
        }

        [TestMethod]
        public void OriginalCoverArtUrl_CoverArtUrlIsRemoteUrlAndHasASizeAttribute_ReturnsTheUrlWithTheSizeAttribute500()
        {
            _subject.CoverArtUrl = "http://asda.com?as=1&size=70";

            _subject.OriginalCoverArtUrl.Should().Be("http://asda.com?as=1&size=500");
        }

        [TestInitialize]
        public void Setup()
        {
            _subject = new PlaylistItem();
        }

        #endregion
    }
}