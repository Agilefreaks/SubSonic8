namespace SubLastFmTests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SubLastFm;
    using SubLastFmTests.Mocks;

    [TestClass]
    public class LastFmServiceTests
    {
        private LastFmService _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new LastFmService(new MockLastFmConfigurationProvider());
        }

        [TestMethod]
        public void GetArtistDetails_Always_ReturnsAGetArtistDetailsResultWithTheGivenArtistName()
        {
            var getArtistDetailsResult = _subject.GetArtistDetails("testArtist");

            getArtistDetailsResult.ArtistName.Should().Be("testArtist");
        }
    }
}