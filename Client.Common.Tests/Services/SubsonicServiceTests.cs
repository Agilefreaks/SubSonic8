﻿using Client.Common.Results;
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
                                        ServiceUrl = "http://test",
                                        Username = "test",
                                        Password = "test"
                                    }
            };
        }

        [TestMethod]
        public void GetRootIndexAlwaysReturnsAGetIndexResult()
        {
            var result = _subject.GetRootIndex();

            result.Should().BeOfType<GetRootResult>();
        }

        [TestMethod]
        public void CtorShouldFunctions()
        {
            _subject.GetMusicDirectory.Should().NotBeNull();
            _subject.GetRootIndex.Should().NotBeNull();
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
            _subject.GetCoverArtForId("test").Should().Contain(string.Format("&size={0}", (int) ImageType.Thumbnail));
        }

        [TestMethod]
        public void GetCoverARtForIdWhenParamterIsNotNullAndImageTypeIsOriginalReturnsStringContainigSize()
        {
            _subject.GetCoverArtForId("test", ImageType.Original)
                    .Should()
                    .Contain(string.Format("&size={0}", (int) ImageType.Original));
        }
    }
}
