using Client.Common.Models;
using Client.Common.Models.Subsonic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Models.Subsonic
{
    [TestClass]
    public class ExpandedArtistTests
    {
        ExpandedArtist _subject;

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
    }
}