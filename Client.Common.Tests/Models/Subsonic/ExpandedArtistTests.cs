namespace Client.Common.Tests.Models.Subsonic
{
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class ExpandedArtistTests
    {
        #region Fields

        private ExpandedArtist _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Setup()
        {
            _subject = new ExpandedArtist();
        }

        [TestMethod]
        public void TypePropertyAlwaysReturnsNavigableTypeEnumArtist()
        {
            _subject.Type.Should().Be(SubsonicModelTypeEnum.Artist);
        }

        #endregion
    }
}