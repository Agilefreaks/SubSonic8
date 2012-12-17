using Client.Common.Models.Subsonic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Models.Subsonic
{
    [TestClass]
    public class MusicDirectoryTests
    {
        private MusicDirectory _subject;

        [TestInitialize]
         public void Setup()
        {
            _subject = new MusicDirectory();
        }

        [TestMethod]
        public void GetDescription_Always_ReturnsTheCorrectValue()
        {
            _subject.Name = "test";
            _subject.Id = 2;

            var description = _subject.GetDescription();

            description.Item1.Should().Be("test");
            description.Item2.Should().Be("2");
        }
    }
}