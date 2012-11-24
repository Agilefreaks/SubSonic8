using Client.Common.Models;
using Client.Common.Models.Subsonic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Models.Subsonic
{
    [TestClass]
    public class MusicDirectoryChildTests
    {
        private MusicDirectoryChild _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new MusicDirectoryChild();
        }

        [TestMethod]
        public void Type_IsDirectoryTrue_ReturnsMusicDirectory()
        {
            _subject.IsDirectory = true;

            _subject.Type.Should().Be(SubsonicModelTypeEnum.MusicDirectory);
        }

        [TestMethod]
        public void Type_IsDirectoryFalseAndIsVideoTrue_ReturnsVideo()
        {
            _subject.IsDirectory = false;
            _subject.IsVideo = true;

            _subject.Type.Should().Be(SubsonicModelTypeEnum.Video);
        }

        [TestMethod]
        public void Type_IsDirectoryFalseAndIsVideoFalse_ReturnsSong()
        {
            _subject.IsDirectory = false;
            _subject.IsVideo = false;

            _subject.Type.Should().Be(SubsonicModelTypeEnum.Song);
        }
    }
}