namespace Client.Common.Tests.Models.Subsonic
{
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class MusicDirectoryTests
    {
        #region Fields

        private MusicDirectory _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void GetDescription_Always_ReturnsTheCorrectValue()
        {
            _subject.Name = "test";
            _subject.Id = 2;

            var description = _subject.GetDescription();

            description.Item1.Should().Be("test");
            description.Item2.Should().Be(string.Empty);
        }

        [TestInitialize]
        public void Setup()
        {
            _subject = new MusicDirectory();
        }

        #endregion
    }
}