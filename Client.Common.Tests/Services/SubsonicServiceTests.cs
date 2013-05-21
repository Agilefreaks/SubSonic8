namespace Client.Common.Tests.Services
{
    using System;
    using Client.Common.Results;
    using Client.Common.Services;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class SubsonicServiceTests
    {
        #region Fields

        private SubsonicService _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Setup()
        {
            _subject = new SubsonicService
            {
                Configuration =
                    new SubsonicServiceConfiguration
                    {
                        BaseUrl = "http://test",
                        Username = "test",
                        Password = "test"
                    }
            };
        }

        [TestMethod]
        public void CtorShouldInitializeFunctions()
        {
            _subject.GetMusicDirectory.Should().NotBeNull();
            _subject.GetMusicFolders.Should().NotBeNull();
            _subject.GetAlbum.Should().NotBeNull();
            _subject.Search.Should().NotBeNull();
        }

        [TestMethod]
        public void GetCoverARtForIdWhenParamterIsNotNullAndImageTypeIsOriginalReturnsStringContainigSize()
        {
            _subject.GetCoverArtForId("test", ImageType.Original)
                    .Should()
                    .Contain(string.Format("&size={0}", (int)ImageType.Original));
        }

        [TestMethod]
        public void GetCoverArtForIdWhenParameterIsNotNullOrEmptyReturnsUrlThatContainThumbnailSize()
        {
            _subject.GetCoverArtForId("test").Should().Contain(string.Format("&size={0}", (int)ImageType.Thumbnail));
        }

        [TestMethod]
        public void GetCoverArtForIdWhenParameterIsNullReturnsStringEmpty()
        {
            _subject.GetCoverArtForId(null).Should().Be(SubsonicService.CoverArtPlaceholder);
        }

        [TestMethod]
        public void GetCoverArtForIdWhenParameterIsStringEmptyReturnsStringEmpty()
        {
            _subject.GetCoverArtForId(string.Empty).Should().Be(SubsonicService.CoverArtPlaceholder);
        }

        [TestMethod]
        public void GetCoverArtForIdWhenParamterIsNotNullOrEmptyReturnsFormattedUrlAsString()
        {
            _subject.GetCoverArtForId("test").Should().Contain("&id=test");
        }

        [TestMethod]
        public void GetIndex_Always_ReturnsAGetIndexResult()
        {
            var result = _subject.GetIndex(5);

            result.Should().BeOfType<GetIndexResult>();
        }

        [TestMethod]
        public void GetMusicFoldersAlwaysReturnsAGetRootResult()
        {
            var result = _subject.GetMusicFolders();

            result.Should().BeOfType<GetRootResult>();
        }

        [TestMethod]
        public void GetUriForVideoStartingAt_Always_ReturnsTheOriginalUriModifiedToHaveTheGivenTimeOffsetParameter()
        {
            var input =
                new Uri("http://google.com/stream/stream.ts?id=30437&hls=true&timeOffset=0&duration=10&maxBitRate=50");

            var uriForVideoStartingAt = _subject.GetUriForVideoStartingAt(input, 100);

            var expectedUri =
                new Uri("http://google.com/stream/stream.ts?id=30437&hls=true&timeOffset=100&duration=10&maxBitRate=50");
            uriForVideoStartingAt.Should().Be(expectedUri);
        }

        [TestMethod]
        public void
            GetUriForVideoStartingAt_Always_ReturnsTheOriginalUriModifiedToHaveTheGivenTimeOffsetParameterRoundedToTheClosestSmallerInteger()
        {
            var input =
                new Uri("http://google.com/stream/stream.ts?id=30437&hls=true&timeOffset=0&duration=10&maxBitRate=50");

            var uriForVideoStartingAt = _subject.GetUriForVideoStartingAt(input, 100.7963545);

            var expectedUri =
                new Uri("http://google.com/stream/stream.ts?id=30437&hls=true&timeOffset=100&duration=10&maxBitRate=50");
            uriForVideoStartingAt.Should().Be(expectedUri);
        }

        [TestMethod]
        public void GetUriForVideoWithId_Always_ReturnsAUrlContainingTheConfigurationBaseUrl()
        {
            var uriForVideoWithId = _subject.GetUriForVideoWithId(1);

            uriForVideoWithId.ToString().Should().StartWith(_subject.Configuration.BaseUrl);
        }

        [TestMethod]
        public void GetUriForVideoWithId_Always_ReturnsAUrlPointingToTheStreamResource()
        {
            var uriForVideoWithId = _subject.GetUriForVideoWithId(1);

            uriForVideoWithId.ToString().Should().Contain("stream/stream.ts");
        }

        [TestMethod]
        public void GetUriForVideoWithId_Always_ReturnsAUrlContainingTheGivenId()
        {
            var uriForVideoWithId = _subject.GetUriForVideoWithId(3);

            uriForVideoWithId.ToString().Should().Contain("id=3");
        }

        [TestMethod]
        public void GetUriForVideoWithId_Always_SpecifiesTheVideoShouldBeHls()
        {
            var uriForVideoWithId = _subject.GetUriForVideoWithId(3);

            uriForVideoWithId.ToString().Should().Contain("hls=true");
        }

        [TestMethod]
        public void GetUriForVideoWithId_NoTimeOffsetGiven_ShouldSetTimeOffsetTo0()
        {
            var uriForVideoWithId = _subject.GetUriForVideoWithId(3);

            uriForVideoWithId.ToString().Should().Contain("timeOffset=0");
        }

        [TestMethod]
        public void GetUriForVideoWithId_TimeOffsetGiven_ShouldSetTimeOffset()
        {
            var uriForVideoWithId = _subject.GetUriForVideoWithId(3, 20);

            uriForVideoWithId.ToString().Should().Contain("timeOffset=20");
        }

        [TestMethod]
        public void GetUriForVideoWithId_MaxBitRateGiven_ShouldSetMaxBitRate()
        {
            var uriForVideoWithId = _subject.GetUriForVideoWithId(3, 20, 200);

            uriForVideoWithId.ToString().Should().Contain("maxBitRate=200");
        }

        [TestMethod]
        public void GetUriForFileWithId_Should_ReturnAUriPoitingAtTheBaseUrl()
        {
            var uriForFileWithId = _subject.GetUriForFileWithId(1);

            uriForFileWithId.ToString().Should().StartWith(_subject.Configuration.BaseUrl);
        }

        [TestMethod]
        public void GetUriForFileWithId_Should_AccessTheStreamResource()
        {
            var uriForFileWithId = _subject.GetUriForFileWithId(1);

            uriForFileWithId.ToString().Should().Contain("stream.view");
        }

        [TestMethod]
        public void GetUriForFileWithId_Should_SetTheUsernameAndPassword()
        {
            var uriForFileWithId = _subject.GetUriForFileWithId(1);

            uriForFileWithId.ToString().Should().Contain("u=" + _subject.Configuration.Username);
            uriForFileWithId.ToString().Should().Contain("p=" + _subject.Configuration.EncodedPassword);
        }

        [TestMethod]
        public void GetUriForFileWithId_Should_TreyToGetTheFileWithTheGivenId()
        {
            var uriForFileWithId = _subject.GetUriForFileWithId(3);

            uriForFileWithId.ToString().Should().Contain("id=3");
        }

        [TestMethod]
        public void HasValidSubsonicUrlWhenConfigurationBaseUrlIsEmptyReturnsFalse()
        {
            _subject.Configuration = new SubsonicServiceConfiguration { BaseUrl = string.Empty };

            _subject.HasValidSubsonicUrl.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidSubsonicUrlWhenConfigurationBaseUrlIsNotEmptyReturnsTrue()
        {
            _subject.Configuration = new SubsonicServiceConfiguration { BaseUrl = "http://test.com" };

            _subject.HasValidSubsonicUrl.Should().BeTrue();
        }

        [TestMethod]
        public void HasValidSubsonicUrlWhenConfigurationIsNullReturnsFalse()
        {
            _subject.Configuration = null;

            _subject.HasValidSubsonicUrl.Should().BeFalse();
        }

        [TestMethod]
        public void Ping_Always_ReturnsAPingResult()
        {
            _subject.Ping().Should().BeOfType<PingResult>();
        }

        #endregion
    }
}