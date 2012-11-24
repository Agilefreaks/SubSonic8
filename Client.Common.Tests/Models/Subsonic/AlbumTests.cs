﻿using Client.Common.Models;
using Client.Common.Models.Subsonic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Models.Subsonic
{
    [TestClass]
    public class AlbumTests
    {
        private Album _subject;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new Album();
        }

        [TestMethod]
        public void InstanceShouldImplementIIdentifiableEntity()
        {
            _subject.Should().BeAssignableTo<IIdentifiableEntity>();
        }

        [TestMethod]
        public void InstanceShouldImplementINavigableEntity()
        {
            _subject.Should().BeAssignableTo<INavigableEntity>();
        }

        [TestMethod]
        public void TypePropertyAlwaysReturnsNavigableTypeEnumAlbum()
        {
            _subject.Type.Should().Be(NavigableTypeEnum.Album);
        }
    }
}
