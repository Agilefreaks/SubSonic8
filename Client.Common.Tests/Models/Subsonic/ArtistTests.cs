namespace Client.Common.Tests.Models.Subsonic
{
    using System;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class ArtistTests
    {
        #region Fields

        private Artist _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void GetDescription_Always_ReturnsATupleWithBothItemsEqualToTheArtistName()
        {
            const string Name = "test_n";
            _subject.Name = Name;

            _subject.GetDescription().Should().Be(new Tuple<string, string>(Name, Name));
        }

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
            _subject = new Artist();
        }

        [TestMethod]
        public void TypePropertyAlwaysReturnsNavigableTypeEnumMusicDirectory()
        {
            _subject.Type.Should().Be(SubsonicModelTypeEnum.MusicDirectory);
        }

        #endregion
    }
}