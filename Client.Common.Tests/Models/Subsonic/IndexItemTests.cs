namespace Client.Common.Tests.Models.Subsonic
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class IndexItemTests
    {
        #region Public Methods and Operators

        private IndexItem _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new IndexItem();
        }

        [TestMethod]
        public void GetDescription_Always_ReturnsATupleWithTheNameAndArtistsCount()
        {
            _subject.Artists = new List<Artist> { new Artist(), new Artist() };
            _subject.Name = "test_n";

            _subject.GetDescription().Should().Be(new Tuple<string, string>("test_n", "2 artists"));
        }

        #endregion
    }
}