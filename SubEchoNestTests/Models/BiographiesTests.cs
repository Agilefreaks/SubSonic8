namespace SubEchoNestTests.Models
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SubEchoNest.Models;

    [TestClass]
    public class BiographiesTests
    {
        private Biographies _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new Biographies();
        }

        [TestMethod]
        public void Constructor_Always_InitializesTheItemsList()
        {
            _subject.Items.Should().NotBeNull();
        }

        [TestMethod]
        public void PreferredBiography_ItemsIsEmpty_ReturnsNull()
        {
            _subject.PreferredBiography.Should().BeNull();
        }

        [TestMethod]
        public void PreferredBiography_ItemsHasOneElement_ReturnsTheOneElement()
        {
            var biography = new Biography();
            _subject.Items.Add(biography);

            _subject.PreferredBiography.Should().Be(biography);
        }

        [TestMethod]
        public void PreferredBiography_ItemsHasMoreThenOneBiographyAndOneIsFromLastFM_ReturnsTheBiographyFromLastFm()
        {
            var biography1 = new Biography { Site = "wikipedia" };
            var biography2 = new Biography { Site = "last.fm" };
            _subject.Items.AddRange(new[] { biography1, biography2 });

            _subject.PreferredBiography.Should().Be(biography2);
        }

        [TestMethod]
        public void PreferredBiography_ItemsHasMoreThenOneBiographyAndNoneIsFromLastFMButOneIsFromWikipedia_ReturnsTheBiographyFromWikipedia()
        {
            var biography1 = new Biography { Site = "test" };
            var biography2 = new Biography { Site = "wikipedia" };

            _subject.Items.AddRange(new[] { biography1, biography2 });

            _subject.PreferredBiography.Should().Be(biography2);
        }

        [TestMethod]
        public void PreferredBiography_ItemsHasMoreThenOneBiographyAndNoneIsFromLastFMButOrWikipedia_ReturnsTheBiographyWithTheLongestText()
        {
            var biography1 = new Biography { Site = "test", Text = "test123" };
            var biography2 = new Biography { Site = "test2", Text = "test1234" };

            _subject.Items.AddRange(new[] { biography1, biography2 });

            _subject.PreferredBiography.Should().Be(biography2);
        }
    }
}
