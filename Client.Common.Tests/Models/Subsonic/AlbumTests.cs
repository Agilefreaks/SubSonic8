namespace Client.Common.Tests.Models.Subsonic
{
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class AlbumTests
    {
        #region Fields

        private Album _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void InstanceShouldImplementIIdentifiableEntity()
        {
            _subject.Should().BeAssignableTo<IId>();
        }

        [TestMethod]
        public void InstanceShouldImplementINavigableEntity()
        {
            _subject.Should().BeAssignableTo<ISubsonicModel>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new Album();
        }

        [TestMethod]
        public void TypePropertyAlwaysReturnsNavigableTypeEnumAlbum()
        {
            _subject.Type.Should().Be(SubsonicModelTypeEnum.Album);
        }

        #endregion
    }
}