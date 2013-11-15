namespace SubEchoNestTests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SubEchoNest;
    using SubEchoNestTests.Mocks;

    [TestClass]
    public class EchoNestServiceTests
    {
        private EchoNestService _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new EchoNestService(new MockLastFmConfigurationProvider());
        }

        [TestMethod]
        public void GetArtistDetails_Always_ReturnsAGetArtistDetailsResultWithTheGivenArtistName()
        {
            var getBiographiesResult = _subject.GetArtistBiographies("testArtist");

            getBiographiesResult.ArtistName.Should().Be("testArtist");
        }
    }
}