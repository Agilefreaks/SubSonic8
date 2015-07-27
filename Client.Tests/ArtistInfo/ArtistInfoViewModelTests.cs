namespace Client.Tests.ArtistInfo
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using global::Common.Mocks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SubEchoNest.Models;
    using SubLastFm.Models;
    using Subsonic8.ArtistInfo;
    using Subsonic8.Framework.Extensions;
    using Biography = SubEchoNest.Models.Biography;
    
    [TestClass]
    public class ArtistInfoViewModelTests
    {
        private ArtistInfoViewModel _subject;
        private MockLastFmService _mockLastFmService;
        private MockEchoNestService _mockEchoNestService;

        [TestInitialize]
        public void Setup()
        {
            _mockLastFmService = new MockLastFmService(new MockLastFmConfigurationProvider());
            _mockEchoNestService = new MockEchoNestService();
            _subject = new ArtistInfoViewModel
            {
                LastFmService = _mockLastFmService,
                EchoNestService = _mockEchoNestService,
            };
        }

        #region Populate

        [TestMethod]
        public async Task Populate_Always_TriesToGetTheArtistInfoUsingTheParameterPropertyValue()
        {
            _subject.Parameter = "testArtist";
            _mockLastFmService.SetupGetArtistDetails(artistName =>
            {
                artistName.Should().Be("testArtist");
                return new MockGetArtistDetailsResult(artistName);
            });

            await _subject.Populate();

            _mockLastFmService.GetArtistDetailsCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Populate_ArtistDetailsHasImages_SetsTheLargestArtistImageOnTheViewModel()
        {
            var image1 = new Image { Size = ImageSizeEnum.Medium, UrlString = "http://test.com/" };
            var image2 = new Image { Size = ImageSizeEnum.Large, UrlString = "http://test2.com/" };
            var images = new List<Image> { image1, image2 };
            var artistDetails = new ArtistDetails { Images = images };
            _mockLastFmService.SetupGetArtistDetails(artistName => new MockGetArtistDetailsResult(artistName)
            {
                GetResultFunc = () => artistDetails
            });

            await _subject.Populate();

            _subject.ArtistImage.Should().Be("http://test2.com/");
        }

        [TestMethod]
        public async Task Populate_ArtistDetailsDoesNotHaveImages_SetsThePlaceHolderImageAsTheArtistImage()
        {
            var artistDetails = new ArtistDetails { Images = new List<Image>() };
            _mockLastFmService.SetupGetArtistDetails(artistName => new MockGetArtistDetailsResult(artistName)
            {
                GetResultFunc = () => artistDetails
            });

            await _subject.Populate();

            _subject.ArtistImage.Should().Be(GetArtistDetailsResultExtensionMethods.CoverArtPlaceholder);
        }

        [TestMethod]
        public async Task Populate_Always_TriesToGetTheArtistBiographyUsingTheParameterPropertyValue()
        {
            _subject.Parameter = "testArtist";
            _mockEchoNestService.SetupGetArtistBiographies(artistName =>
            {
                artistName.Should().Be("testArtist");
                return new MockGetBiographiesResult(artistName)
                {
                    GetResultFunc = () => new Biographies()
                };
            });

            await _subject.Populate();

            _mockEchoNestService.GetArtistBiographiesCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Populate_BiographiesHasItems_SetsTheBestBiographyOnTheViewModel()
        {
            var biography1 = new Biography { Text = "test1", Site = "wikipedia" };
            var biography2 = new Biography { Text = "test2", Site = "last.fm" };
            var biographies = new Biographies { Items = new List<Biography> { biography1, biography2 } };
            _mockEchoNestService.SetupGetArtistBiographies(artistName => new MockGetBiographiesResult(artistName)
            {
                GetResultFunc = () => biographies
            });

            await _subject.Populate();

            _subject.Biography.Text.Should().Be(biography2.Text);
        }

        [TestMethod]
        public async Task Populate_ArtistDetailsDoesNotHaveBiography_SetsThePlaceHolderTextAsTheArtistBiography()
        {
            var biographies = new Biographies();
            _mockEchoNestService.SetupGetArtistBiographies(artistName => new MockGetBiographiesResult(artistName)
            {
                GetResultFunc = () => biographies
            });

            await _subject.Populate();

            _subject.ArtistImage.Should().Be(GetArtistDetailsResultExtensionMethods.CoverArtPlaceholder);
        }

        #endregion
    }
}