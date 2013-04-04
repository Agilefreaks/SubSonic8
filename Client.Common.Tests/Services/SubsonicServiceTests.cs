using System;
using Client.Common.Results;
using Client.Common.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Services
{
    [TestClass]
    public class SubsonicServiceTests
    {
        private SubsonicService _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new SubsonicService
            {
                Configuration = new SubsonicServiceConfiguration
                                    {
                                        BaseUrl = "http://test",
                                        Username = "test",
                                        Password = "test"
                                    }
            };
        }

        [TestMethod]
        public void GetMusicFoldersAlwaysReturnsAGetRootResult()
        {
            var result = _subject.GetMusicFolders();

            result.Should().BeOfType<GetRootResult>();
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
        public void GetCoverArtForIdWhenParameterIsNotNullOrEmptyReturnsUrlThatContainThumbnailSize()
        {
            _subject.GetCoverArtForId("test").Should().Contain(string.Format("&size={0}", (int)ImageType.Thumbnail));
        }

        [TestMethod]
        public void GetCoverARtForIdWhenParamterIsNotNullAndImageTypeIsOriginalReturnsStringContainigSize()
        {
            _subject.GetCoverArtForId("test", ImageType.Original)
                    .Should()
                    .Contain(string.Format("&size={0}", (int)ImageType.Original));
        }

        [TestMethod]
        public void HasValidSubsonicUrlWhenConfigurationIsNullReturnsFalse()
        {
            _subject.Configuration = null;

            _subject.HasValidSubsonicUrl.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidSubsonicUrlWhenConfigurationBaseUrlIsEmptyReturnsFalse()
        {
            _subject.Configuration = new SubsonicServiceConfiguration
                                         {
                                             BaseUrl = string.Empty
                                         };

            _subject.HasValidSubsonicUrl.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidSubsonicUrlWhenConfigurationBaseUrlIsNotEmptyReturnsTrue()
        {
            _subject.Configuration = new SubsonicServiceConfiguration
                                         {
                                             BaseUrl = "http://test.com"
                                         };

            _subject.HasValidSubsonicUrl.Should().BeTrue();
        }

        [TestMethod]
        public void GetUriForVideoStartingAt_Always_ReturnsTheOriginalUriModifiedToHaveTheGivenTimeOffsetParameter()
        {
            var input = new Uri("http://google.com/stream/stream.ts?id=30437&hls=true&timeOffset=0&duration=10&maxBitRate=50");

            var uriForVideoStartingAt = _subject.GetUriForVideoStartingAt(input, 100);

            var expectedUri = new Uri("http://google.com/stream/stream.ts?id=30437&hls=true&timeOffset=100&duration=10&maxBitRate=50");
            uriForVideoStartingAt.Should().Be(expectedUri);
        }        
        
        [TestMethod]
        public void GetUriForVideoStartingAt_Always_ReturnsTheOriginalUriModifiedToHaveTheGivenTimeOffsetParameterRoundedToTheClosestSmallerInteger()
        {
            var input = new Uri("http://google.com/stream/stream.ts?id=30437&hls=true&timeOffset=0&duration=10&maxBitRate=50");

            var uriForVideoStartingAt = _subject.GetUriForVideoStartingAt(input, 100.7963545);

            var expectedUri = new Uri("http://google.com/stream/stream.ts?id=30437&hls=true&timeOffset=100&duration=10&maxBitRate=50");
            uriForVideoStartingAt.Should().Be(expectedUri);
        }

        [TestMethod]
        public void GetIndex_Always_ReturnsAGetIndexResult()
        {
            var result = _subject.GetIndex(5);

            result.Should().BeOfType<GetIndexResult>();
        }
    }
}
