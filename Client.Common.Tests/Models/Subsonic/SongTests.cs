namespace Client.Common.Tests.Models.Subsonic
{
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class SongTests
    {
        #region Fields

        private Song _subject;

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
            _subject = new Song();
        }

        [TestMethod]
        public void TypePropertyAlwaysReturnsNavigableTypeEnumAlbum()
        {
            _subject.Type.Should().Be(SubsonicModelTypeEnum.Song);
        }

        #endregion
    }
}