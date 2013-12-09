namespace SubLastFmTests.Models
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SubLastFm.Models;

    [TestClass]
    public class BiographyTests
    {
        private Biography _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new Biography();
        }

        [TestMethod]
        public void SetPublishedDateString_Always_SetsPublishedDate()
        {
            _subject.PublishDateString = "Mon, 24 Sep 2012 09:15:24 +0000";

            _subject.PublishDate.Year.Should().Be(2012);
            _subject.PublishDate.Month.Should().Be(9);
            _subject.PublishDate.Day.Should().Be(24);
        }
    }
}
