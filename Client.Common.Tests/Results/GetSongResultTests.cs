﻿using Client.Common.Results;
using Client.Common.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class GetSongResultTests
    {
        private IGetSongResult _subject;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new GetSongResult(new SubsonicServiceConfiguration {ServiceUrl = "{0}{1}{2}"}, 12);
        }

        [TestMethod]
        public void ViewNameShouldBegetMusicDirectory()
        {
            _subject.ViewName.Should().Be("getSong.view");
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&id=12");
        }
    }
}