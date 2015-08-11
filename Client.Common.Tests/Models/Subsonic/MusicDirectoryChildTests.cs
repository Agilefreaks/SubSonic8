namespace Client.Common.Tests.Models.Subsonic
{
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class MusicDirectoryChildTests
    {
        #region Fields

        private MusicDirectoryChild _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Setup()
        {
            _subject = new MusicDirectoryChild();
        }

        [TestMethod]
        public void Type_IsDirectoryFalseAndIsVideoFalse_ReturnsSong()
        {
            _subject.IsDirectory = false;
            _subject.IsVideo = false;

            _subject.Type.Should().Be(SubsonicModelTypeEnum.Song);
        }

        [TestMethod]
        public void Type_IsDirectoryFalseAndIsVideoTrue_ReturnsVideo()
        {
            _subject.IsDirectory = false;
            _subject.IsVideo = true;

            _subject.Type.Should().Be(SubsonicModelTypeEnum.Video);
        }

        [TestMethod]
        public void Type_IsDirectoryTrue_ReturnsMusicDirectory()
        {
            _subject.IsDirectory = true;

            _subject.Type.Should().Be(SubsonicModelTypeEnum.MusicDirectory);
        }

        [TestMethod]
        public void GetDescription_TypeIsMusicDirectory_ReturnsADescriptionWithTheSecondItemEmpty()
        {
            _subject.IsDirectory = true;

            var result = _subject.GetDescription();

            result.Item2.Should().Be(string.Empty);
        }

        [TestMethod]
        public void GetDescription_TypeIsVideo_ReturnsADescriptionWithTheSecondItemEmpty()
        {
            _subject.IsDirectory = false;
            _subject.IsVideo = true;

            var result = _subject.GetDescription();

            result.Item2.Should().Be(_subject.Artist);
        }

        [TestMethod]
        public void GetDescription_TypeIsSong_ReturnsADescriptionWithTheSecondItemEmpty()
        {
            _subject.IsDirectory = false;
            _subject.IsVideo = false;
            
            var result = _subject.GetDescription();

            result.Item2.Should().Be(_subject.Artist);
        }

        #endregion
    }
}