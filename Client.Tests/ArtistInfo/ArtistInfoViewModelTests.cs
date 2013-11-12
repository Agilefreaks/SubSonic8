namespace Client.Tests.ArtistInfo
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using global::Common.Services;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SubLastFm.Models;
    using Subsonic8.ArtistInfo;
    using Subsonic8.Framework.Services;

    [TestClass]
    public class ArtistInfoViewModelTests
    {
        private ArtistInfoViewModel _subject;
        private MockLastFmService _mockLastFmService;

        [TestInitialize]
        public void Setup()
        {
            _mockLastFmService = new MockLastFmService(new LastFmConfigurationProvider());
            _subject = new ArtistInfoViewModel
            {
                LastFmService = _mockLastFmService,
                HtmlTransformService = new HtmlTransformService()
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
                return new MockGetArtistDetailsResult(artistName)
                {
                    GetResultFunc = () => new ArtistDetails()
                };
            });

            await _subject.Populate();

            _mockLastFmService.GetArtistDetailsCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Populate_Always_SetsTheObtainedArtistDetailsOnTheViewModel()
        {
            var artistDetails = new ArtistDetails();
            _mockLastFmService.SetupGetArtistDetails(artistName => new MockGetArtistDetailsResult(artistName)
            {
                GetResultFunc = () => artistDetails
            });

            await _subject.Populate();

            _subject.ArtistDetails.Should().Be(artistDetails);
        }

        [TestMethod]
        public async Task Populate_Always_SetsThePlaintextBiographyOfTheArtistOnTheViewModel()
        {
            var biography = new Biography { Content = "<a href='http://google.com'>test 1</a>" };
            var artistDetails = new ArtistDetails { Biography = biography };
            _mockLastFmService.SetupGetArtistDetails(artistName => new MockGetArtistDetailsResult(artistName)
            {
                GetResultFunc = () => artistDetails
            });

            await _subject.Populate();

            _subject.Biography.Should().Be("test 1");
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

            _subject.ArtistImage.Should().Be(ArtistInfoViewModel.CoverArtPlaceholder);
        }

        #endregion
    }
}